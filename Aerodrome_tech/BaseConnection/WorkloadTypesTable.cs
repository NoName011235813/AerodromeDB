/*
 *	Класс: WorkloadTypesTable
 *	Класс предназначенный для взаимодействия с таблицей
 *	видов объемов работ
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *  Задание:
 *		Описывает работу с таблицей видов объемов работ в 
 *		базе данных.
 *		
 *  Реализует интерфейс IDbTable.
 *		
 *	Переменные, описанные в данном классе:
 *		Connection - соединение с базой данных;
 *		Adapter - взаимодействие с таблицей в базе данных;
 *		Data - источник данных, куда будет записываться информация
 *		из базы данных;
 *		Columns - описание столбцов таблицы;
 *		TableName - название таблицы в базе данных.
 *		
 *	Подпрограммы, описанные в данном классе:
 *		WorkloadTypesTable - конструктор класса;
 *		Save - функция сохранения изменений в таблице, возвращает
 *		true, если сохранение прошло успешно, false в противоположном случае;
 *		Cancel - процедура отмены несохраненных изменений;
 *		AddRow - процедура добавления строки к таблице;
 *		Fill - процедура отправки запроса на выборку данных.
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace BaseConnection {

	class WorkloadTypesTable: IDbTable {
				
		public MySqlConnection Connection { get; set; }
		public MySqlDataAdapter Adapter { get; set; } 
		public DataSet Data { get; set; }

		public QueryColumns Columns { get; set; }	// Определение столбцов

		string tableName = "workload";
		public string TableName { get { return tableName; } }

		/*
		 *	Конструктор класса
		 *	
		 *	Формальный парметр:
		 *		Conn - ссылка на соединение в рамках которого
		 *		будет выполняться работа с БД.
		 */
		public WorkloadTypesTable(MySqlConnection Conn) {

			Columns = new QueryColumns();

													// Имена столбцов в таблице
			Columns.Add("id", "Ид", MySqlDbType.Int32, true,  0);
			Columns.Add("(select mt.name from maint_types mt where mt.id = w.maint_type_id)", "Тип", MySqlDbType.VarChar, false,  20);
			Columns.Add("w.name", "Название", MySqlDbType.VarChar, false,  10);
							
			Connection = Conn;

													// Составление запроса на
													// выборку
			Adapter = new MySqlDataAdapter(QueryUtils.CreateSelectQ(Columns, tableName + " w "));
			Adapter.SelectCommand.Connection = Connection;

			Data = new DataSet();
													// Заполнение кэша
			Fill();
			
													// Составление DML запросов
			Adapter.InsertCommand = Connection.CreateCommand();
			Adapter.InsertCommand.CommandText = 
				"insert into " + tableName +
				" (maint_type_id, name)" +
				" values ((select id from maint_types where name = @pname), @pwlname)";

			Adapter.InsertCommand.Parameters.Add("pname", MySqlDbType.VarChar, 20, "Тип");
			Adapter.InsertCommand.Parameters.Add("pwlname", MySqlDbType.VarChar, 10, "Название");


			Adapter.UpdateCommand = Connection.CreateCommand();
			Adapter.UpdateCommand.CommandText =
				"update " + tableName +
				" set maint_type_id = (select id from maint_types where name = @pname)," + 
				" name = @pwlname " +
				"where id = @pid";

			Adapter.UpdateCommand.Parameters.Add("pid", MySqlDbType.Int32, 0, "Ид");
			Adapter.UpdateCommand.Parameters.Add("pname", MySqlDbType.VarChar, 20, "Тип");
			Adapter.UpdateCommand.Parameters.Add("pwlname", MySqlDbType.VarChar, 10, "Название");
		
			Adapter.DeleteCommand = QueryUtils.CreateDeleteQ(Columns, tableName);
			Adapter.DeleteCommand.Connection = Connection;
			
		}

		/*
		 *	Функция сохранения внесённых в таблицу изменений
		 *	
		 *	Локальные переменные:
		 *		temp - временный источник данных, что хранит строки, что были
		 *		добавлены или изменены;
		 *		Row - временная переменная, используемая в цикле foreach,
		 *		хранит строки из temp;
		 *		fkAdapter - адаптер, используемый для проверки
		 *		внешних ключей;
		 *		uniqueAdapter - адаптер, используемый для проверки
		 *		уникальности столбцов;
		 *		fkSet - источник данных, используемый для проверки
		 *		внешних ключей.
		 */
		public bool Save() { 

													// Внесённые изменения
			DataSet temp = Data.GetChanges(DataRowState.Added | DataRowState.Modified);


			if (temp != null) {

													// Определение и создание 
													// объектов, необходимых для
													// проверки внешних ключей
				MySqlDataAdapter fkAdapter = new MySqlDataAdapter(
					"select id from maint_types where name = @pname",
					Connection
				);

				fkAdapter.SelectCommand.Parameters.Add("pname", MySqlDbType.VarChar);

													// Определение и создание 
													// объектов, необходимых для
													// проверки уникальности
				MySqlDataAdapter uniqueAdapter = new MySqlDataAdapter(
					"select id from workload where name = @pname",
					Connection
				);

				uniqueAdapter.SelectCommand.Parameters.Add("pname", MySqlDbType.VarChar);

				DataTable fkSet = new DataTable();

				foreach (DataRow Row in temp.Tables[0].Rows) {
										
													// Проверка на пустые поля
					if (
						Row["Тип"] == DBNull.Value || 
						Row["Название"] == DBNull.Value
					) {
						MessageBox.Show("Вы оставили пустые поля, сохранение отменено");
						return false;
					}

													// Проверка значений
					if (
						!(
							CheckUtil.CheckStringLength(Row, "Название", 10) &&
							CheckUtil.CheckStringLength(Row, "Тип", 20)
						)
					) 
						return false;
					
													// Проверка внешнего ключа
					fkAdapter.SelectCommand.Parameters["pname"].Value = Row["Тип"];

					fkSet.Rows.Clear();
					fkSet.Columns.Clear();

					if (fkAdapter.Fill(fkSet) == 0) {
						Connection.Close();
						MessageBox.Show("Тип объема работ не был найден");
						return false;
					}

													// Проверка на уникальность
													// столбца "Название"
					if (!CheckUtil.CheckUniqueValue(uniqueAdapter, "pname", Data, Row, "Название")) {
						Connection.Close();
						MessageBox.Show("Название должно быть уникальным");
						return false;
					}

				}

			}

			Connection.Close();

													// Отправка изменений
			try {
				Adapter.Update(Data.Tables[0]);
				return true;
			} catch {
				MessageBox.Show("Произошла непредвиденная ошибка при сохранении");
				return false;
			}

		}

													// Отмена изменений
		public void Cancel() { 
			Data.Tables[0].RejectChanges();
		}

													// Добавление строки
		public void AddRow() { 
			try {
				Data.Tables[0].Rows.Add(Data.Tables[0].NewRow());
			} catch {
				MessageBox.Show("Произошла ошибка при добавлении строки в таблицу");
			}
		}

													// Выполнение запроса выборки
		public void Fill() {
			Data.Clear();
			try {
				Adapter.Fill(Data);
				Connection.Close();
			} catch {
				Connection.Close();
				MessageBox.Show("Произошла ошибка при заполнении таблицы");
				return;
			}

													// Настройки столбцов
			Data.Tables[0].Columns["Ид"].ReadOnly = true;
		}

	}

}

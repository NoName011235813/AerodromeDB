/*
 *	Класс: WorksHistoryTable
 *	Класс предназначенный для взаимодействия с таблицей
 *	истории работ
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *  Задание:
 *		Описывает работу с таблицей истории работ в 
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
 *		WorksHistoryTable - конструктор класса;
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

	class WorksHistoryTable: IDbTable {
				
		public MySqlConnection Connection { get; set; }
		public MySqlDataAdapter Adapter { get; set; } 
		public DataSet Data { get; set; }

		public QueryColumns Columns { get; set; }	// Определение столбцов

		string tableName = "works_history";
		public string TableName { get { return tableName; } }

		/*
		 *	Конструктор класса
		 *	
		 *	Формальный парметр:
		 *		Conn - ссылка на соединение в рамках которого
		 *		будет выполняться работа с БД.
		 */
		public WorksHistoryTable(MySqlConnection Conn) {

			Columns = new QueryColumns();

													// Имена столбцов в таблице
			Columns.Add("id", "Ид", MySqlDbType.Int32, true,  0);
			Columns.Add("(select wl.name from workload wl where wl.id = wh.workload_id)", "Тип_объема_работ", MySqlDbType.VarChar, false, 10);
			Columns.Add("(select p.registration_number from plane p where p.id = wh.plane_id)", "Рег_номер_самолёта", MySqlDbType.VarChar, false, 10);
			Columns.Add("(select g.group_name from tech_group g where g.id = wh.t_group_id)", "Группа_техников", MySqlDbType.VarChar, false, 25);
			Columns.Add("wh.work_date", "Дата", MySqlDbType.Date, false, 0);
			Columns.Add("wh.more", "Дополнительно", MySqlDbType.Text, false, 512);
						
			Connection = Conn;

													// Составление запроса на
													// выборку
			Adapter = new MySqlDataAdapter(QueryUtils.CreateSelectQ(Columns, tableName + " wh "));
			Adapter.SelectCommand.Connection = Connection;

			Data = new DataSet();
													// Заполнение кэша
			Fill();
													// Составление DML запросов
			Adapter.InsertCommand = Connection.CreateCommand();
			Adapter.InsertCommand.CommandText = 
				"insert into " + tableName +
				" (workload_id, plane_id, t_group_id, work_date, more)" + 
				" values(" +
					"(select id from workload where name = @pwname), " + 
					"(select id from plane where registration_number = @preg_num), " +
					"(select id from tech_group where group_name = @ptname), " +
					"@pwork_date, @pmore" +
				")";

			Adapter.InsertCommand.Parameters.Add("pwname", MySqlDbType.VarChar, 10, "Тип_объема_работ");
			Adapter.InsertCommand.Parameters.Add("preg_num", MySqlDbType.VarChar, 10, "Рег_номер_самолёта");
			Adapter.InsertCommand.Parameters.Add("ptname", MySqlDbType.VarChar, 25, "Группа_техников");
			Adapter.InsertCommand.Parameters.Add("pwork_date", MySqlDbType.Date, 0, "Дата");
			Adapter.InsertCommand.Parameters.Add("pmore", MySqlDbType.Text, 512, "Дополнительно");
						

			Adapter.UpdateCommand = Connection.CreateCommand();
			Adapter.UpdateCommand.CommandText =	
				"update " + tableName +
				" set " +
					"workload_id = (select id from workload where name = @pwname), " +
					"plane_id = (select id from plane where registration_number = @preg_num), " +
					"t_group_id = (select id from tech_group where group_name = @ptname), " +
					"work_date = @pwork_date, " +
					"more = @pmore " +
				"where id = @pid";

			Adapter.UpdateCommand.Parameters.Add("pid", MySqlDbType.Int32, 0, "Ид");
			Adapter.UpdateCommand.Parameters.Add("pwname", MySqlDbType.VarChar, 10, "Тип_объема_работ");
			Adapter.UpdateCommand.Parameters.Add("preg_num", MySqlDbType.VarChar, 10, "Рег_номер_самолёта");
			Adapter.UpdateCommand.Parameters.Add("ptname", MySqlDbType.VarChar, 25, "Группа_техников");
			Adapter.UpdateCommand.Parameters.Add("pwork_date", MySqlDbType.Date, 0, "Дата");
			Adapter.UpdateCommand.Parameters.Add("pmore", MySqlDbType.Text, 512, "Дополнительно");


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
				MySqlDataAdapter fkAdapter = new MySqlDataAdapter();
				fkAdapter.SelectCommand = Connection.CreateCommand();

				fkAdapter.SelectCommand.Parameters.Add("pwname", MySqlDbType.VarChar);
				fkAdapter.SelectCommand.Parameters.Add("preg_num", MySqlDbType.VarChar);
				fkAdapter.SelectCommand.Parameters.Add("ptname", MySqlDbType.VarChar);

				DataTable fkSet = new DataTable();
				
				foreach (DataRow Row in temp.Tables[0].Rows) { 

													// Проверка на пустые поля
					if (
						Row["Тип_объема_работ"] == DBNull.Value || 
						Row["Рег_номер_самолёта"] == DBNull.Value || 
						Row["Группа_техников"] == DBNull.Value || 
						Row["Дата"] == DBNull.Value
					) {
						MessageBox.Show("Вы оставили пустые поля, сохранение отменено");
						return false;
					}

													// Проверка значений 
					if (
						!(
							CheckUtil.CheckStringLength(Row, "Тип_объема_работ", 10) &&
							CheckUtil.CheckStringLength(Row, "Рег_номер_самолёта", 10) &&
							CheckUtil.CheckStringLength(Row, "Группа_техников", 25)
						)
					) 
						return false;

													// Проверка даты
					DateTime tempDate;
					if (!DateTime.TryParse(Row["Дата"].ToString(), out tempDate) || (tempDate > DateTime.Now.Date) ) {
						MessageBox.Show("Проверьте введенные даты");
						return false;
					}

													// Проверка внешнего ключа
					fkSet.Rows.Clear();
					fkSet.Columns.Clear();

					fkAdapter.SelectCommand.CommandText = "select id from workload where name = @pwname";
					fkAdapter.SelectCommand.Parameters["pwname"].Value = Row["Тип_объема_работ"];

					if (fkAdapter.Fill(fkSet) == 0) {
						Connection.Close();
						MessageBox.Show("Тип объема работ не был найден");
						return false;
					}

					fkSet.Rows.Clear();
					fkSet.Columns.Clear();

					fkAdapter.SelectCommand.CommandText = "select id from plane where registration_number = @preg_num";
					fkAdapter.SelectCommand.Parameters["preg_num"].Value = Row["Рег_номер_самолёта"];

					if (fkAdapter.Fill(fkSet) == 0) {
						Connection.Close();
						MessageBox.Show("Регистрационный номер самолёта не был найден");
						return false;
					}

					fkSet.Rows.Clear();
					fkSet.Columns.Clear();

					fkAdapter.SelectCommand.CommandText = "select id from tech_group where group_name = @ptname";
					fkAdapter.SelectCommand.Parameters["ptname"].Value = Row["Группа_техников"];

					if (fkAdapter.Fill(fkSet) == 0) {
						Connection.Close();
						MessageBox.Show("Группа техников не была найдена");
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

													// Выполнение запроса 
													// выборки
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

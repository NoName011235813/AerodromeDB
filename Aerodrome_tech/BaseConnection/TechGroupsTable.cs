/*
 *	Класс: TechGroupsTable
 *	Класс предназначенный для взаимодействия с таблицей
 *	групп техников
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *  Задание:
 *		Описывает работу с таблицей групп техников в 
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
 *		TechGroupsTable - конструктор класса;
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

	class TechGroupsTable: IDbTable {

		public MySqlConnection Connection { get; set; }
		public MySqlDataAdapter Adapter { get; set; } 
		public DataSet Data { get; set; }

		public QueryColumns Columns { get; set; }	// Определение столбцов

		string tableName = "tech_group";
		public string TableName { get { return tableName; } }

		/*
		 *	Конструктор класса
		 *	
		 *	Формальный парметр:
		 *		Conn - ссылка на соединение в рамках которого
		 *		будет выполняться работа с БД.
		 */
		public TechGroupsTable(MySqlConnection Conn) {

			Columns = new QueryColumns();

													// Имена столбцов в таблице
			Columns.Add("id", "Ид", MySqlDbType.Int32, true,  0);
			Columns.Add("tg.group_name", "Название_группы", MySqlDbType.VarChar, false, 25);
			Columns.Add("(select l.name from group_leader l where l.id = tg.leader_id)", "Руководитель", MySqlDbType.VarChar, false,  70);
			Columns.Add("tg.about", "Допольнительно", MySqlDbType.Text, false,  512);
						
			Connection = Conn;

													// Составление запроса на
													// выборку
			Adapter = new MySqlDataAdapter(QueryUtils.CreateSelectQ(Columns, tableName + " tg "));
			Adapter.SelectCommand.Connection = Connection;

			Data = new DataSet();
													// Заполнение кэша
			Fill();

													// Составление DML запросов
			Adapter.InsertCommand = Connection.CreateCommand();
			Adapter.InsertCommand.CommandText =
				"insert into " + tableName +
				" (group_name, leader_id, about)" +
				" values (@pgroup_name, (select id from group_leader where name = @plname), @pabout)";

			Adapter.InsertCommand.Parameters.Add("pgroup_name", MySqlDbType.VarChar, 25, "Название_группы");
			Adapter.InsertCommand.Parameters.Add("plname", MySqlDbType.VarChar, 70, "Руководитель");
			Adapter.InsertCommand.Parameters.Add("pabout", MySqlDbType.Text, 512, "Допольнительно");

			
			Adapter.UpdateCommand = Connection.CreateCommand();
			Adapter.UpdateCommand.CommandText =
				"update " + tableName +
				" set group_name = @pgroup_name," + 
				" leader_id = (select id from group_leader where name = @plname)," +
				" about = @pabout " +
				"where id = @pid";

			Adapter.UpdateCommand.Parameters.Add("pid", MySqlDbType.Int32, 0, "Ид");
			Adapter.UpdateCommand.Parameters.Add("pgroup_name", MySqlDbType.VarChar, 25, "Название_группы");
			Adapter.UpdateCommand.Parameters.Add("plname", MySqlDbType.VarChar, 70, "Руководитель");
			Adapter.UpdateCommand.Parameters.Add("pabout", MySqlDbType.Text, 512, "Допольнительно");

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
		 *		внешних ключей;
		 *		IdAdapter - адаптер для работы со внешними ключами, выбранными
		 *		пользователем;
		 *		ChoosedIdTable - таблица для хранения строк с внешними включами;
		 *		CForm - форма выбора записи;
		 *		ChoosedId - ид выбранной пользователем записи;
		 *		NewRow - новая строка, добавляемая в таблицу ChoosedIdTable;
		 *		ThatRowInMainSet - рассматриваемая строка в основном источнике данных.
		 */
		public bool Save() { 
	
													// Внесённые изменения
			DataSet temp = Data.GetChanges(DataRowState.Added | DataRowState.Modified);

			if (temp != null) { 

													// Определение и создание 
													// объектов, необходимых для
													// проверки внешних ключей
				MySqlDataAdapter fkAdapter = new MySqlDataAdapter(
					"select * from group_leader where name = @plname",
					Connection
				);

				fkAdapter.SelectCommand.Parameters.Add("plname", MySqlDbType.VarChar);

													// Определение и создание 
													// объектов, необходимых для
													// проверки уникальности

				MySqlDataAdapter uniqueAdapter = new MySqlDataAdapter(
					"select id from tech_group where group_name = @pgroup_name",
					Connection
				);

				uniqueAdapter.SelectCommand.Parameters.Add("pgroup_name", MySqlDbType.VarChar);

				DataTable fkSet = new DataTable();


													// Объекты для работы 
													// с внешними ключами
													// выбранными пользователем
				MySqlDataAdapter IdAdapter = new MySqlDataAdapter();

				IdAdapter.InsertCommand = Connection.CreateCommand();
				IdAdapter.InsertCommand.CommandText =
					"insert into " + tableName +
					" (group_name, leader_id, about)" +
					" values (@pgroup_name, @pleader_id, @pabout)";
			
				IdAdapter.InsertCommand.Parameters.Add("pgroup_name", MySqlDbType.VarChar, 25, "Название_группы");
				IdAdapter.InsertCommand.Parameters.Add("pleader_id", MySqlDbType.Int32, 0, "Руководитель");
				IdAdapter.InsertCommand.Parameters.Add("pabout", MySqlDbType.Text, 512, "Допольнительно");

			
				IdAdapter.UpdateCommand = Connection.CreateCommand();
				IdAdapter.UpdateCommand.CommandText =
					"update " + tableName +
					" set group_name = @pgroup_name," + 
					" leader_id = @pleader_id," +
					" about = @pabout " +
					"where id = @pid";

				IdAdapter.UpdateCommand.Parameters.Add("pid", MySqlDbType.Int32, 0, "Ид");
				IdAdapter.UpdateCommand.Parameters.Add("pgroup_name", MySqlDbType.VarChar, 25, "Название_группы");
				IdAdapter.UpdateCommand.Parameters.Add("pleader_id", MySqlDbType.Int32, 0, "Руководитель");
				IdAdapter.UpdateCommand.Parameters.Add("pabout", MySqlDbType.Text, 512, "Допольнительно");

				DataSet ChoosedIdTable = new DataSet();

				ChoosedIdTable.Tables.Add(new DataTable());

				ChoosedIdTable.Tables[0].Columns.Add("Ид");
				ChoosedIdTable.Tables[0].Columns.Add("Название_группы");
				ChoosedIdTable.Tables[0].Columns.Add("Допольнительно");
				ChoosedIdTable.Tables[0].Columns.Add("Руководитель");
				
				foreach (DataRow Row in temp.Tables[0].Rows) { 
			
													// Проверка на пустые поля
					if (
						Row["Название_группы"] == DBNull.Value || 
						Row["Руководитель"] == DBNull.Value
					) {
						MessageBox.Show("Вы оставили пустые поля, сохранение отменено");
						return false;
					}

													// Проверка значений
					if (
						!(
							CheckUtil.CheckStringLength(Row, "Название_группы", 25) &&
							CheckUtil.CheckStringLength(Row, "Руководитель", 70)
						)
					) 
						return false;

													// Проверка на уникальность
													// столбца "Название_группы"
					if (!CheckUtil.CheckUniqueValue(uniqueAdapter, "pgroup_name", Data, Row, "Название_группы")) {
						Connection.Close();
						MessageBox.Show("Название должно быть уникальным");
						return false;
					}

													// Проверка внешнего ключа
					fkAdapter.SelectCommand.Parameters["plname"].Value = Row["Руководитель"];
					
					fkSet.Rows.Clear();
					fkSet.Columns.Clear();

					fkAdapter.Fill(fkSet);
					Connection.Close();

					if (fkSet.Rows.Count == 0) {
						MessageBox.Show("Руководитель не был найден");
						return false;
					} else if (fkSet.Rows.Count > 1) {
													
													// Если было возвращено
													// несколько записей,
													// вызвать окно выбора
													// записей
						ChooseForm CForm = new ChooseForm(fkSet);

						if (CForm.ShowDialog() == DialogResult.OK) {

							int ChoosedId = CForm.ChoosedId;

													// Создание новой строки
													// и её заполнение
													// данными
							DataRow NewRow = ChoosedIdTable.Tables[0].NewRow();

							NewRow["Ид"] = Row["Ид"];
							NewRow["Название_группы"] = Row["Название_группы"];
							NewRow["Допольнительно"] = Row["Допольнительно"];
							NewRow["Руководитель"] = ChoosedId;

							ChoosedIdTable.Tables[0].Rows.Add(NewRow);

						} else {
							MessageBox.Show("Вы отменили ввод");
							return false;
						}

						CForm.Dispose();
					}			
													
				}

													// Передача значений 
													// с выбранным Ид
				if (ChoosedIdTable.Tables[0].Rows.Count > 0) {

					foreach (DataRow Row in ChoosedIdTable.Tables[0].Rows) {
						
													// Очистка нынешнего 
													// статуса (переводит 
													// в unchanged)

													// Без этого нельзя 
													// переопределять статус 
													// строки
						Row.AcceptChanges();

													// Нахождение строки в 
													// основном источнике 
													// данных по 
													// первичному ключу
						DataRow ThatRowInMainSet = Data.Tables[0].Rows.Find(Row[0]);

													// Определение статуса 
													// строки для определения 
													// используемого далее запроса
						if (ThatRowInMainSet.RowState == DataRowState.Added)
							Row.SetAdded();
						else
							Row.SetModified();

													// Для этих строк будет 
													// выполняться запрос с
													// конкретным ид руководителя
						ThatRowInMainSet.AcceptChanges();

					}

													// Отправка изменений, для
													// которых был выбран ид
													// руководителя
					try {
						IdAdapter.Update(ChoosedIdTable.Tables[0]);
					} catch (Exception e) {
						MessageBox.Show("Произошла непредвиденная ошибка при сохранении " + e.Message);
						return false;
					}
				}

			}

			Connection.Close();
													// Отправка остальных
													// изменений
			try {
				Adapter.Update(Data.Tables[0]);
				return true;
			} catch (Exception e) {
				MessageBox.Show("Произошла непредвиденная ошибка при сохранении " + e.Message);
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
			} catch (Exception e) {
				MessageBox.Show("Произошла ошибка при добавлении строки в таблицу " + e.Message);
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

													// Значение первичного 
													// ключа нельзя изменить
			Data.Tables[0].Columns["Ид"].ReadOnly = true;
			Data.Tables[0].Columns["Ид"].AutoIncrement = true;

													// Устанавливаем 
													// первичный ключ для 
													// таблицы локально
			Data.Tables[0].PrimaryKey = new DataColumn[] { Data.Tables[0].Columns["Ид"] };
		}

	}

}

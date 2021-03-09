/*
 *	Класс: PlaneModelsTable
 *	Класс предназначенный для взаимодействия с таблицей
 *	моделей самолётов
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *  Задание:
 *		Описывает работу с таблицей моделей самолётов в 
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
 *		PlaneModelsTable - конструктор класса;
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

	class PlaneModelsTable : IDbTable {

		public MySqlConnection Connection { get; set; }
		public MySqlDataAdapter Adapter { get; set; } 
		public DataSet Data { get; set; }

		public QueryColumns Columns { get; set; }	// Определение столбцов

		string tableName = "plane_model";

		public string TableName { get { return tableName; } }

		/*
		 *	Конструктор класса
		 *	
		 *	Формальный парметр:
		 *		Conn - ссылка на соединение в рамках которого
		 *		будет выполняться работа с БД.
		 */
		public PlaneModelsTable(MySqlConnection Conn) {

			Columns = new QueryColumns();

													// Имена столбцов в таблице
			Columns.Add("id", "Ид", MySqlDbType.Int32, true,  0);
			Columns.Add("name", "Название", MySqlDbType.VarChar, false,  100);
			Columns.Add("manufacturer", "Производитель", MySqlDbType.VarChar, false,  70);
			Columns.Add("fuel_tank_volume", "Емкость_топливных_баков_Л", MySqlDbType.Int32, false,  0);
			Columns.Add("places_num", "Количество_мест", MySqlDbType.Int32, false,  0);
			Columns.Add("max_lifting_capacity", "Максимальный_коммерческий_вес_КГ", MySqlDbType.Int32, false, 0);
						
			Connection = Conn;
													// Составление запроса на
													// выборку
			Adapter = new MySqlDataAdapter(QueryUtils.CreateSelectQ(Columns, tableName));
			Adapter.SelectCommand.Connection = Connection;

			Data = new DataSet();
													// Заполнение кэша
			Fill();
													// Составление DML запросов
			Adapter.InsertCommand = QueryUtils.CreateInsertQ(Columns, tableName);
			Adapter.InsertCommand.Connection = Connection;

			Adapter.UpdateCommand = QueryUtils.CreateUpdateQ(Columns, tableName);
			Adapter.UpdateCommand.Connection = Connection;

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
		 *		хранит строки из temp.
		 */
		public bool Save() { 

													// Внесённые изменения
			DataSet temp = Data.GetChanges(DataRowState.Added | DataRowState.Modified);
			
			if (temp != null)
				foreach (DataRow Row in temp.Tables[0].Rows) {

													// Проверка на пустые поля
					if (
						Row["Производитель"] == DBNull.Value || 
						Row["Название"] == DBNull.Value ||
						Row["Емкость_топливных_баков_Л"] == DBNull.Value ||
						Row["Количество_мест"] == DBNull.Value ||
						Row["Максимальный_коммерческий_вес_КГ"] == DBNull.Value
					) {
						MessageBox.Show("Вы оставили пустые поля, сохранение отменено");
						return false;
					}

													// Проверка значений
					if (
						!(
							CheckUtil.CheckStringLength(Row, "Производитель", 65) &&
							CheckUtil.CheckStringLength(Row, "Название", 95) &&
							CheckUtil.CheckIntAndLength(Row, "Емкость_топливных_баков_Л", 1, 1000000) &&
							CheckUtil.CheckIntAndLength(Row, "Количество_мест", 1, 1000) &&
							CheckUtil.CheckIntAndLength(Row, "Максимальный_коммерческий_вес_КГ", 1, 700000)
						)
					)
						return false;
					
				}
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

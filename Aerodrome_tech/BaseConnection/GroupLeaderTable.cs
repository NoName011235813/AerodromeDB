/*
 *	Класс: GroupLeaderTable
 *	Класс предназначенный для взаимодействия с таблицей
 *	руководителей команд техников
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *  Задание:
 *		Описывает работу с таблицей руководителей команд техников в 
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
 *		GroupLeaderTable - конструктор класса;
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
using System.Text.RegularExpressions;

namespace BaseConnection {

	class GroupLeaderTable: IDbTable {

		public MySqlConnection Connection { get; set; }
		public MySqlDataAdapter Adapter { get; set; } 
		public DataSet Data { get; set; }

		public QueryColumns Columns { get; set; }	// Определение столбцов

		string tableName = "group_leader";
		public string TableName { get { return tableName; } }

		/*
		 *	Конструктор класса
		 *	
		 *	Формальный парметр:
		 *		Conn - ссылка на соединение в рамках которого
		 *		будет выполняться работа с БД.
		 */
		public GroupLeaderTable(MySqlConnection Conn) {

			Columns = new QueryColumns();

													// Имена столбцов в таблице
			Columns.Add("id", "Ид", MySqlDbType.Int32, true,  0);
			Columns.Add("name", "ФИО", MySqlDbType.VarChar, false,  70);
			Columns.Add("phone", "Телефон", MySqlDbType.VarChar, false,  14);
			Columns.Add("experience", "Стаж", MySqlDbType.Int32, false,  0);
			Columns.Add("passport", "Паспорт", MySqlDbType.VarChar, false,  14);
			Columns.Add("about", "Доп_информация", MySqlDbType.Text, false,  512);
						
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
		 *		хранит строки из temp;
		 *		tempAdapter - адаптер, используемый для проверки уникальности.
		 */
		public bool Save() { 

													// Внесённые изменения
			DataSet temp = Data.GetChanges(DataRowState.Added | DataRowState.Modified);
			
			if (temp != null) {
													// Определение и создание 
													// объектов, необходимых для
													// проверки уникальности
				MySqlDataAdapter tempAdapter = new MySqlDataAdapter(
					"select id from group_leader where passport = @ppassport",
					Connection
				);
				tempAdapter.SelectCommand.Parameters.Add("ppassport", MySqlDbType.VarChar);

				foreach (DataRow Row in temp.Tables[0].Rows) {

													// Проверка на пустые поля
					if (
						Row[Columns["name"].GridName] == DBNull.Value || 
						Row[Columns["phone"].GridName] == DBNull.Value ||
						Row[Columns["experience"].GridName] == DBNull.Value ||
						Row[Columns["passport"].GridName] == DBNull.Value
					) {
						MessageBox.Show("Вы оставили пустые поля, сохранение отменено");
						return false;
					}

													// Проверка значений
					if (
						!(
							CheckUtil.CheckStringLength(Row, Columns["name"].GridName, 65) &&
							CheckUtil.CheckStringLength(Row, Columns["phone"].GridName, 14) &&
							CheckUtil.CheckIntAndLength(Row, Columns["experience"].GridName, 2, 40) &&
							CheckUtil.CheckStringLength(Row, Columns["passport"].GridName, 14) 
						)
					)
						return false;
					
													// Проверка телефона 
													// на правильность 
													// формата
					Regex rgx = new Regex("^" + Regex.Escape("+") + "{0,1}[0-9]{10,13}$");

					if (!rgx.IsMatch(Row[Columns["phone"].GridName].ToString())) {
						MessageBox.Show("Телефон введен неправилно");
						return false;
					}

													// Проверка на уникальность
													// столбца "Паспорт"
					if (!CheckUtil.CheckUniqueValue(tempAdapter, "ppassport", Data, Row, "Паспорт")) {
						Connection.Close();
						MessageBox.Show("Паспорт должен быть уникальным");
						return false;
					}
										
				}
					
				Connection.Close();

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

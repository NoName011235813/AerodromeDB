/*
 *	Классы: ColumnDescription, QueryColumns, QueryUtils
 *	Классы используемые для облегчения работы с запросами
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *  Задание:
 *		Классы для описания столбцов, что входят в запрос, 
 *		и для составления на основе данного описания запросов 
 *		на выборку, добавления, изменения и удаления (для 
 *		запросов, что ссылаются на несколько таблиц только на выборку
 *		и удаления).
 *		
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace BaseConnection {

	/*
	 *	Класс: ColumnDescription
	 *	Описывает информацию о столбце запроса
	 *	
	 *	Язык: C#
	 *	Разработал: Штирбу Алин, ТИП - 62
	 *	Дата: 04.02.2021г
	 * 
	 *  Задание:
	 *		Описать информацию, необходимую для автоматического составления
	 *		запросов.
	 *	
	 *	Переменные, описанные в данном классе:
	 *		GridName - будущее название столбца в программе;
	 *		Type - тип данных столбца в базе данных;
	 *		Length - размерность столбца;
	 *		PrimaryKey - является ли столбец первичным ключом.
	 *		
	 *	Подпрограмма, описанная в данном классе:
	 *		ColumnDescription - конструктор класса.
	 * 
	 */

	public class ColumnDescription {
			
		public string GridName { get; set; }
		public MySqlDbType Type { get; set; }
		public int Length { get; set; }
		public bool PrimaryKey { get; set; }

		/*
		 *	Конструктор класса
		 *	
		 *	Формальные параметры:
		 *		gname - название столбца в сетке;
		 *		type - тип столбца;
		 *		pk - является ли столбец первичным ключом;
		 *		length - размерность информации в столбце.
		 */
		public ColumnDescription(string gname, MySqlDbType type, bool pk, int length) {
			GridName = gname;
			Type = type;
			PrimaryKey = pk;
			Length = length;
		}

	}

	/*
	 *	Класс: QueryColumns
	 *	Описывает набор ColumnDescription и работу с ним
	 *	
	 *	Язык: C#
	 *	Разработал: Штирбу Алин, ТИП - 62
	 *	Дата: 04.02.2021г 
	 * 
	 *  Задание:
	 *		Описать набор столбцов, что будут входить в будущие запросы.
	 *	
	 *	Переменные, описанные в данном классе:
	 *		Columns - словарь, что содержит данные в виде <название_столбца_в_БД:описание_столбца>;
	 *		Count - количество описанных столбцов;
	 *		Keys - список названий столбцов в базе данных.
	 *				
	 *  Индексатор, описанный в данном классе:
	 *		this[string key] - возвращение описания столбца по названию.
	 * 
	 *	Подпрограмма, описанная в данном классе:
	 *		Add - используется для добавления нового столбца.
	 * 
	 */

	public class QueryColumns {
			
		Dictionary<string, ColumnDescription> Columns = new Dictionary<string, ColumnDescription>();

													// Количество столбцов
		public int Count {
			get { return Columns.Count; }
		}

													// Возврат списка ключей 
													// (имён таблиц в БД)
		public Dictionary<string, ColumnDescription>.KeyCollection Keys {
			get { return Columns.Keys; }
		}

		/*
		 *	Индексатор
		 *	
		 *	Формальный параметр:
		 *		key - имя столбца в запросе.
		 */
		public ColumnDescription this[string key] {
			get { return Columns[key]; }
			set { Columns[key] = value; }
		}

		/*
		 *	Добавление нового столбца
		 *	
		 *	Формальные параметры:
		 *		dbName - название столбца в запросе;
		 *		gName - название столбца в сетке вывода;
		 *		type - тип столбца;
		 *		pk - является ли столбец первичным ключом;
		 *		length - размерность информации в столбце.
		 */
		public void Add(string dbName, string gName, MySqlDbType type, bool pk, int length) {
			Columns.Add(dbName, new ColumnDescription(gName, type, pk, length));
		}

	}




	/*
	 *	Класс: QueryUtils
	 *	Статический класс, что описывает методы для автоматического создания запросов
	 *	
	 *	Язык: C#
	 *	Разработал: Штирбу Алин, ТИП - 62
	 *	Дата: 04.02.2021г 
	 * 
	 *  Задание:
	 *		Описать методы для создания запросов на основании набора
	 *		описаний столбцов.
	 *	
	 *	Функции, описанные в данном классе:
	 *		CreateSelectQ - создаёт запрос на выборку на основании переданного набора
	 *		столбцов, имя таблицы и необязательного условия, возвращает экземпляр 
	 *		класса MySqlCommand;
	 *		CreateInsertQ - создаёт запрос на добавление на основании переданного набора
	 *		столбцов и имени таблицы, возвращает экземпляр класса MySqlCommand;
	 *		CreateUpdateQ - создаёт запрос на изменение на основании переданного набора
	 *		столбцов и имени таблицы, возвращает экземпляр класса MySqlCommand;
	 *		CreateDeleteQ - создаёт запрос на удаление на основании переданного набора
	 *		столбцов и имени таблицы, возвращает экземпляр класса MySqlCommand;
	 *		GetConnection - создает объект соединения с базой данных, возвращает
	 *		экземпляр класса MySqlConnection.
	 * 
	 */
	static class QueryUtils {

		/* 
		 *	Функция для создания запроса на выборку
		 *	
		 *	Формальные параметры:
		 *		cols - набор столбцов запроса;
		 *		table - название таблицы;
		 *		etc - условия выборки, по умолчанию пустое значение.
		 *		
		 *	Локальные переменные:
		 *		QueryText - текст запроса;
		 *		key - временная переменная, используемая в 
		 *		цикле foreach для перебора названий столбцов;
		 *		i - счётчик.
		 */
		static public MySqlCommand CreateSelectQ(QueryColumns cols, string table, string etc = null) {
													// Проверка на пустоту
													// передаваемого 
													// набора
			if (cols.Count <= 0)
				return null;

			string QueryText = "select ";

			int i = 1;
													// Составление текста
													// запроса 
			foreach (string key in cols.Keys) {
				QueryText += key + " as " + cols[key].GridName;
													// Если не последний
													// столбец, добавляет
													// запятую
				if (i < cols.Count) {
					QueryText += ", ";
					i++;
				}

			}
													// Указание таблицы
			QueryText += " from " + table;
													
													// Если есть условия выборки,
													// добавляет
			if (etc != null)
				QueryText += " where " + etc; 

			return new MySqlCommand(QueryText);
		}

		/* 
		 *	Функция для создания запроса на добавление
		 *	
		 *	Формальные параметры:
		 *		cols - набор столбцов запроса;
		 *		table - название таблицы.
		 *		
		 *	Локальные переменные:
		 *		QueryText - текст запроса;
		 *		key - временная переменная, используемая в 
		 *		цикле foreach для перебора названий столбцов;
		 *		i - счётчик;
		 *		temp - временная переменная, содержит возвращаемый запрос;
		 *		ValueStr - строка значений запроса.
		 */
		static public MySqlCommand CreateInsertQ(QueryColumns cols, string table) {
			
			if (cols.Count <= 0)
				return null;

			string QueryText = "insert into " + table + " (";
			string ValueStr = " values (";

													// Возвращаемый запрос
			MySqlCommand temp = new MySqlCommand();

			int i = 1;

			foreach (string key in cols.Keys) {
													// Параллельное 
													// добавление названий 
													// таблиц и параметров
				if (!cols[key].PrimaryKey) {
					QueryText += key;
					ValueStr += "@p" + key;
					temp.Parameters.Add("p" + key, cols[key].Type, cols[key].Length, cols[key].GridName);

													// Добавление запятых, 
													// если не последний 
													// столбец 
					if (i < cols.Count) {
						QueryText += ", ";
						ValueStr += ", ";
					}
						
				}

				i++;

			}

			QueryText += ")";
			ValueStr += ")";

			temp.CommandText = QueryText + ValueStr;

			return temp;

		}

		/* 
		 *	Функция для создания запроса на изменение
		 *	
		 *	Формальные параметры:
		 *		cols - набор столбцов запроса;
		 *		table - название таблицы.
		 *		
		 *	Локальные переменные:
		 *		QueryText - текст запроса;
		 *		key - временная переменная, используемая в 
		 *		цикле foreach для перебора названий столбцов;
		 *		i - счётчик;
		 *		temp - временная переменная, содержит возвращаемый запрос;
		 *		WhereStatement - строка условия запроса.
		 */
		static public MySqlCommand CreateUpdateQ(QueryColumns cols, string table) {
			
			if (cols.Count <= 0)
				return null;

			string QueryText = "update " + table + " set ";
			string WhereStatement = " where ";

													// Возвращаемый запрос
			MySqlCommand temp = new MySqlCommand();

			int i = 1;

			foreach (string key in cols.Keys) {

				
				if (!cols[key].PrimaryKey) {
					QueryText += key + " = " + "@p" + key;

													// Добавление запятых, 
													// если не последний 
													// столбец 
					if (i < cols.Count) 
						QueryText += ", ";
					
				} else 
					WhereStatement += key + " = " + "@p" + key;
					
				
				temp.Parameters.Add("p" + key, cols[key].Type, cols[key].Length, cols[key].GridName);

				i++;

			}

			QueryText += WhereStatement;

			temp.CommandText = QueryText;

			return temp;

		}
		
		/* 
		 *	Функция для создания запроса на удаление
		 *	
		 *	Формальные параметры:
		 *		cols - набор столбцов запроса;
		 *		table - название таблицы.
		 *		
		 *	Локальные переменные:
		 *		QueryText - текст запроса;
		 *		key - временная переменная, используемая в 
		 *		цикле foreach для перебора названий столбцов;
		 *		temp - временная переменная, содержит возвращаемый запрос.
		 */
		static public MySqlCommand CreateDeleteQ(QueryColumns cols, string table) {
			
			string QueryText = "delete from " + table + " where ";

			MySqlCommand temp = new MySqlCommand();

			foreach (string key in cols.Keys)
				if (cols[key].PrimaryKey) {
					QueryText += key + " = " + "@p" + key;
					temp.Parameters.Add("p" + key, cols[key].Type, cols[key].Length, cols[key].GridName);
				}

			temp.CommandText = QueryText;

			return temp;

		}
		
		/*
		 *	Функция создания объекта соединения с БД
		 *	
		 *	Локальная переменная:
		 *		Connection - возвращаемое соединение.
		 */
		static public MySqlConnection GetConnection() {
			
			MySqlConnection Connection = new MySqlConnection(
				"Server=localhost;"+
				"Database=aerodrome_tech;"+
				"port=3306;"+
				"User Id=root;"+
				"password=fkby2002"	
			);

			return Connection;
		}

	}

}

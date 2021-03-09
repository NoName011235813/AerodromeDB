/*
 *	Интерфейс: IDbTable
 *	Описывает функционал классов таблиц
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *	Задание:
 *		Описывает функционал, необходимый для взаимодействия 
 *		с основной формой программы.
 *	
 *	Переменные, объявленные в интерфейсе:
 *		Connection - соединение с базой данных;
 *		Adapter - взаимодействие с таблицей в базе данных;
 *		Data - источник данных, куда будет записываться информация
 *		из базы данных;
 *		Columns - описание столбцов таблицы, для автоматического
 *		составления запросов простых таблиц;
 *		TableName - название таблицы в базе данных.
 *		
 *  Подпрограммы, объявленные в интерфейсе:
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

using System.Data;
using MySql.Data.MySqlClient;

namespace BaseConnection {

	interface IDbTable {

		MySqlConnection Connection { get; set; }
		MySqlDataAdapter Adapter { get; set; } 
		DataSet Data { get; set; }

		QueryColumns Columns { get; set; }

		string TableName { get; }

		// bool ReadOnly { get; }

		bool Save();
		void Cancel();
		void AddRow();
		void Fill();

	}

}


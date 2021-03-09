/*
 *	Класс: CheckUtil
 *	Статический класс, методы которого используются для проверки
 *	вводимых пользователем данных
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *	Задание:
 *		Описывает набор статических методов, используемых для проверки вводимых 
 *		пользователем данных.
 *		
 *	Функции, описываемые в данном классе:
 *		CheckStringLength - проверяет длину строки, возвращает true, если 
 *		длина переданной строки меньше переданного числа, false в противоположном случае;
 *		CheckIntAndLength - проверяет является ли переданная строка числом и его вхождение
 *		в переданный диапазон чисел, возвращает true, если строка соответствует описанным
 *		условиям, false в противоположном случае;
 *		CheckUniqueValue - проверяет уникальность значения в столбце в локальном
 *		источнике данных и в базе данных, возвращает true, если значение уникальное, false
 *		в противоположном случае.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;

namespace BaseConnection {

	static class CheckUtil {

		/*
		 *	Функция проверки длины строки
		 *	
		 *	Формальные параметры:
		 *		row - строка из таблицы, где находится проверяемая строка;
		 *		colName - название столбца, где находится проверяемая строка;
		 *		length - число, с которым сравнивается длина строки.
		 */
		public static bool CheckStringLength(DataRow row, string colName, int length) {
			
			if (row[colName].ToString().Length > length) {
				MessageBox.Show("Поле \"" + colName + "\" содержит слишком много символов");
				return false;
			}

			return true;
		}

		/*
		 *	Функция проверки чисел на тип и на вхождение в диапазон
		 *	
		 *	Формальные параметры:
		 *		row - строка из таблицы, где находится проверяемая строка;
		 *		colName - название столбца, где находится проверяемая строка;
		 *		startPos - начало диапазона проверки вхождения;
		 *		endPos - конец диапазона проверки вхождения.
		 *		
		 *	Локальная переменная:
		 *		aInt - временная переменная, используемая для хранения результирующего числа.
		 */
		public static bool CheckIntAndLength(DataRow row, string colName, int startPos, int endPos) {

			int aInt;
													// Проверка на тип
			if (!int.TryParse(row[colName].ToString(), out aInt)) {
				MessageBox.Show("В столбце \"" + colName + "\" содержатся недопустимые значения");
				return false;
			}

													// Проверка на вхождение в диапазон
			if (aInt < startPos || aInt > endPos) {
				MessageBox.Show("В столбце \"" + colName + "\" содержатся значения выходящие за рамки диапазона допустимых значений");
				return false;
			}
		
			return true;

		}
		
		/*
		 *	Функция проверки уникальности значений в столбце
		 *	Параметры:
		 *		uniqueAdapter - объект работы с запросами с веденным запросом выборки;
		 *		paramName - имя параметра, указанного в запросе выборки;
		 *		Data - источник данных, проверяемый на уникальность значений;
		 *		Row - строка, что содержит проверяемое значение;
		 *		ColumnName - название столбца, что проверяется на уникальность.
		 *		
		 *	Локальная переменная:
		 *		uniqueSet - временный источник данных.
		 */
		public static bool CheckUniqueValue(MySqlDataAdapter uniqueAdapter, string paramName, DataSet Data, DataRow Row, string ColumnName) {
			
			uniqueAdapter.SelectCommand.Parameters[paramName].Value = Row[ColumnName];
			
			DataTable uniqueSet = new DataTable();

													// Если запись была добавлена
													// проверка на отстутствие записи
			if (
				(
					(Row.RowState == DataRowState.Added) && 
					(uniqueAdapter.Fill(uniqueSet) > 0 || Data.Tables[0].Select(ColumnName + " = '" + Row[ColumnName] + "'").GetLength(0) > 1)
				) ||
													// Если запись была изменена
													// проверяется на единичность
													// существования
				(
					(Row.RowState == DataRowState.Modified) &&
					(uniqueAdapter.Fill(uniqueSet) > 1 ||  Data.Tables[0].Select(ColumnName + " = '" + Row[ColumnName] + "'").GetLength(0) > 1)
				)
						
			)
				return false;

			return true;

		}
	

	}

}

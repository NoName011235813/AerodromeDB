/*
 *	Класс: AppUser
 *	Класс для описания учетной записи пользователя
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *  Задание:
 *		Хранить данные учетной записи пользователя.
 *		
 *	Переменные, описанные в данном классе:
 *		Login - логин пользователя;
 *		Id - ид пользователя;
 *		Status - уровень прав пользователя.
 *		
 *	Подпрограмма, описанная в данном классе:
 *		AppUser - конструктор класса.
 *		
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseConnection {

	public class AppUser {

		public string Login { get; set; }
		public string Id { get; set; }
		public string Status { get; set; }

		/*
		 *	Конструктор класса
		 *	
		 *	Формальные параметры:
		 *		login - логин пользователя;
		 *		id - ид пользователя;
		 *		status - уровень прав пользователя.
		 */
		public AppUser(string login, string id, string status) {
			Login = login;
			Id = id;
			Status = status;
		}

	}

}

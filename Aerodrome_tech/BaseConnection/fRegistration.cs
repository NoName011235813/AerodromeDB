/*
 *	Форма: fRegistration
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *	Задание: 
 *		Предоставляет пользователю возможность зарегистрироваться
 *		в систему.
 *		
 *	Переменные, используемые в данной форме:
 *		Connection - соединение с базой данных;
 *		AuthForm - хранит ссылку на форму логина, используется
 *		для отображения формы логина.
 * 
 *	Подпрограммы, используемые в данной форме:
 *		fRegistration - конструктор класса;
 *		GetSalt - функция, что составляет соль переданной длины для 
 *		хэширования паролей, возвращает строку;
 *		bSubmit_Click - обработчик нажатия кнопки подтверждения;
 *		bBack_Click - обработчик нажатия кнопки возврата на форму логина;
 *		fRegistration_FormClosing - обработчик закрытия формы.
 *		
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace BaseConnection {

	public partial class fRegistration : Form {
													// Создание соединения
		MySqlConnection Connection = QueryUtils.GetConnection();

													// Ссылка на форму
													// авторизации
		Form AuthForm;
													
		/*
		 *	Конструктор класса
		 * 
		 *	Формальный параметр:
		 *		LoginForm - ссылка на форму авторизации.
		 */
		public fRegistration(Form LoginForm) {

			InitializeComponent();
													// Передача ссылки
			AuthForm = LoginForm;

		}

		/*	
		 *	Функция генерации соли
		 * 
		 *	Формальный параметр:
		 *		codeLength - длина возвращаемой строки, равен 6
		 *		по умолчанию.
		 */
		private string GetSalt(int codeLength = 6) {
			
			string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPRQSTUVWXYZ0123456789";
			string code = "";
			Random rnd = new Random();

			while (code.Length < codeLength)
				code += chars[rnd.Next(0, chars.Length - 1)];

			return code;
		}

		/*
		 *	Обработчик нажатия кнопки подтверждения
		 *	
		 *	Формальные параметры:
		 *		sender - объект, что вызвал метод;
		 *		e - дополнительные свойства.
		 *		
		 *	Локальные переменные:
		 *		login - значение логина;
		 *		password - значение пароля;
		 *		rgx - объект работы с описанным регулярным выражением;
		 *		md5 - объект работы с хэшированием;
		 *		Adapter - объект работы с запросами к серверу;
		 *		Result - таблица, что содержит ответ сервера;
		 *		salt - переменная для хранения соли;
		 *		buffer - временная переменная для хранения строки пароля в 
		 *		виде последовательности байтов;
		 *		hashbyte - хэш от buffer;
		 *		hashToString - объект, для образования строки при переводе
		 *		хэша в строку.
		 */
		private void bSubmit_Click(object sender, EventArgs e) {

													// Значения из полей
			string login = tbLogin.Text.Trim(), password = tbPassword.Text.Trim();

													// Объявление и создание
													// регулярного выражения
			Regex rgx = new Regex("^[a-zA-Z0-9]*$");

													// Объект для работы с
													// хэшированием
			MD5 md5 = MD5.Create();

													// Проверка на пустоту 
													// полей
			if (login.Length == 0 || password.Length == 0) {
				MessageBox.Show("Вы оставили логин или пароль пустым");
				return;
			}

													// Проверка длины введенных
													// данных
			if (login.Length > 20 || password.Length > 20 || tbRepeatPassword.Text.Trim().Length > 20) {
				MessageBox.Show("Пароль и логин могут содержать не более 20 символов");
				return;
			}

													// Проверка по регулярному 
													// выражению
			if (!rgx.IsMatch(login)) {
				MessageBox.Show("Логин может содержать цифры, большие и маленькие буквы латинского алфавита");
				return;
			}

													// Проверка совпадения
													// с повтором пароля
			if (password != tbRepeatPassword.Text.Trim()) {
				MessageBox.Show("Пароли не совпадают");
				return;
			}
													// Поиск пользователей
													// с тем же логином, что и 
													// введенный пользователем
			MySqlDataAdapter Adapter = new MySqlDataAdapter(
				"select id from users where ulogin = @plogin", 
				Connection
			);

			Adapter.SelectCommand.Parameters.Add("plogin", MySqlDbType.VarChar).Value = login;

			DataTable Result = new DataTable();

			try {

				Adapter.Fill(Result);
				Connection.Close();

			} catch (Exception ex) {
				Connection.Close();
				MessageBox.Show("Произошла ошибка при обращении к серверу " + ex.Message);
				return;
			}
			
													// Проверка на существование
													// пользователя с данным 
													// логином
			if (Result.Rows.Count > 0) {
				MessageBox.Show("Пользователь уже существует");
				return;
			}

			
													// Вычисление хэша пароля
			string salt = GetSalt();
			byte[] buffer = Encoding.ASCII.GetBytes(salt + password);
			byte[] hashByte = md5.ComputeHash(buffer);

			StringBuilder hashToString = new StringBuilder();

			for (int i = 0; i < hashByte.Length; i++)
				hashToString.Append(hashByte[i].ToString("X2"));

			password = hashToString.ToString();

													// Передача данных новой
													// учетной записи
			Adapter.InsertCommand = Connection.CreateCommand();
			Adapter.InsertCommand.CommandText = 
				"insert into users (ulogin, usalt, upassword)" +
				"values(@plogin, @psalt, @ppassword)";

			Adapter.InsertCommand.Parameters.Add("plogin", MySqlDbType.VarChar).Value = login;
			Adapter.InsertCommand.Parameters.Add("psalt", MySqlDbType.VarChar).Value = salt;
			Adapter.InsertCommand.Parameters.Add("ppassword", MySqlDbType.VarChar).Value = password;

			try {
				Connection.Open();

				Adapter.InsertCommand.ExecuteNonQuery();

				Connection.Close();

			} catch (Exception ex) {
				Connection.Close();
				MessageBox.Show("Произошла ошибка при обращении к серверу " + ex.Message);
			}
			
			MessageBox.Show("Регистрация прошла успешно");

		}

													// Кнопка - Назад
		private void bBack_Click(object sender, EventArgs e) {
			AuthForm.Show();
			this.Dispose();
		}

													// Завершение работы программы
		private void fRegistration_FormClosing(object sender, FormClosingEventArgs e) {
			Application.Exit();
		}
			
	}

}

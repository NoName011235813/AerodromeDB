/*
 *	Форма: fAuth
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *	Задание: 
 *		Предоставляет пользователю возможность авторизоваться в систему.
 * 
 *	Ожидаемые входные данные:
 *		данные пользователя для входа в систему.
 *		
 *	Переменная, используемая в данной форме:
 *		Connection - соединение с базой данных.
 *		
 *	Подпрограммы, используемые в данной форме:
 *		fAuth - конструктор класса;
 *		bRegister_Click - обработчик нажатия кнопки перехода на форму регистрации;
 *		bSignIn_Click - обработчик нажатия кнопки входа;
 *		fAuth_FormClosing - обработчик закрытия формы.
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

	public partial class fAuth : Form {
													// Создание соединения
		MySqlConnection Connection = QueryUtils.GetConnection();

													// Конструктор класса
		public fAuth() {

			InitializeComponent();

		}

		/*
		 *	Переход на окно регистрации
		 *	
		 *	Локальная переменная:
		 *		fRegister - форма регистрации.
		 */
		private void bRegister_Click(object sender, EventArgs e) {
			this.Hide();
			fRegistration fRegister = new fRegistration(this);
			fRegister.Show();
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
		 *		BasePassword - переменная, что содержит пароль из БД;
		 *		Salt - переменная, для содержит соль;
		 *		InputPass - временная переменная для хранения строки пароля в 
		 *		виде последовательности байтов;
		 *		HashedPass - хэш от InputPass;
		 *		hashToString - объект, для образования строки при переводе
		 *		хэша в строку;
		 *		MainForm - основная форма.
		 */
		private void bSignIn_Click(object sender, EventArgs e) {

													// Значения из полей
			string login = tbLogin.Text.Trim(), password = tbPassword.Text.Trim();

													// Объявление и создание
													// регулярного выражения
			Regex rgx = new Regex("^[a-zA-Z0-9]*$");

													// Проверка на пустоту 
													// полей
			if (login.Length == 0 || password.Length == 0) {
				MessageBox.Show("Вы оставили логин или пароль пустым");
				return;
			}

													// Проверка длины введенных
													// данных
			if (login.Length > 20 || password.Length > 20) {
				MessageBox.Show("Пароль и логин могут содержать не более 20 символов");
				return;
			}

													// Проверка по регулярному 
													// выражению
			if (!rgx.IsMatch(login)) {
				MessageBox.Show("Логин может содержать цифры, большие и маленькие буквы латинского алфавита");
				return;
			}
													// Запрос на данные 
													// пользователя
			MySqlDataAdapter Adapter = new MySqlDataAdapter(
				"select u.id, u.upassword, u.usalt, s.name from users u, statuses s where ulogin = @plogin and s.id = u.status_id",
				Connection
			);

			Adapter.SelectCommand.Parameters.Add("@plogin", MySqlDbType.VarChar).Value = login;

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
			if (Result.Rows.Count == 0) {
				MessageBox.Show("Пароль или логин введён неправильно");
				return;
			}

													// Пароль и соль из ответа
			string BasePassword = Result.Rows[0]["upassword"].ToString();
			string Salt = Result.Rows[0]["usalt"].ToString();

													// Объект работы с 
													// хэшированием
			MD5 md5 = MD5.Create();

													// Вычисление хэша пароля
			byte[] InputPass = Encoding.ASCII.GetBytes(Salt + password);
			byte[] HashedPass = md5.ComputeHash(InputPass);

			StringBuilder hashToString = new StringBuilder();

			for (int i = 0; i < HashedPass.Length; i++)
				hashToString.Append(HashedPass[i].ToString("X2"));

													// Сравнение хэшей
			if (hashToString.ToString() != BasePassword) {
				MessageBox.Show("Пароль или логин введён неправильно");
				return;
			}

													// Переход на основную
													// форму
			FMain MainForm = new FMain(
				new AppUser(login, Result.Rows[0]["id"].ToString(), Result.Rows[0]["name"].ToString()),
				this
			);

			MainForm.Show();
			this.Hide();

		}

													// Завершение работы 
													// программы
		private void fAuth_FormClosing(object sender, FormClosingEventArgs e) {
			Application.Exit();
		}

	}

}

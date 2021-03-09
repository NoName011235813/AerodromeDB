/*
 *	Программа: AerodromeTech
 *	Учебная практика по профессиолнальному модулю
 *		ПМ. 02 Осуществление интеграции программных модулей
 *		
 *	Тема: "Техническое обслуживание самолётов"
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *	Задание: 
 *		Разработать клиент для полноценной работы с базой 
 *		данных по техническому обслуживанию самолётов.
 *	
 * Ожидаемые входные данные:
 *		данные входа пользователя;
 *		информация о моделей самолётов;
 *		информация о самолётах авиакомпании;
 *		информация о видах технического обслуживания;
 *		информация о видах объемов работ в рамках определенного
 *		вида технического обслуживания;
 *		информация о руководителях групп техников;
 *		информация о группах техников;
 *		информация о произведенных работах над самолётами.
 *		
 * Переменные, используемые в данной форме:
 *		Connection - соединение с базой данных;
 *		ActiveTable - указывает на таблицу, с которой производится
 *		работа на данный момент;
 *		BSource - соединяет источник данных с сеткой вывода информации;
 *		User - учетная запись пользователя;
 *		LoginForm - указатель на форму аутентификации;
 *		MaintTypes - таблица типов технического обслуживания;
 *		Models - таблица моделей самолётов;
 *		Leaders - таблица руководиетелей групп техников;
 *		Workload - таблица видов объемов работ;
 *		TechGroups - таблица групп техников;
 *		Planes - таблица самолётов;
 *		WorksHistory - таблица произведенных работ;
 *		Users - таблица пользователей системы.
 *		
 *	Подпрограммы, используемые в данной форме:
 *		FMain - конструктор формы;
 *		DisableImportantFunctions - процедура, для ввода программы
 *		в режим чтения, используется при пользовователе
 *		с уровнем доступа - Гость;
 *		ShowTable - процедура, что выполняет запрос на выборку данных,
 *		для их отображения в сетку;
 *		bMaintTypes_Click - процедура, обработчик нажатия кнопки переключения
 *		на таблицу типов технического обслуживания;
 *		bGroupsLeaders_Click - процедура, обработчик нажатия кнопки переключения
 *		на таблицу руководителей групп техников;
 *		BPlaneModels_Click - процедура, обработчик нажатия кнопки переключения
 *		на таблицу моделей самолётов;
 *		bWorkloads_Click - процедура, обработчик нажатия кнопки переключения
 *		на таблицу видов объема работы;
 *		bTechGroups_Click - процедура, обработчик нажатия кнопки переключения
 *		на таблицу групп техников;
 *		bPlanes_Click - процедура, обработчик нажатия кнопки переключения
 *		на таблицу самолётов;
 *		bWorksHistory_Click - процедура, обработчик нажатия кнопки переключения
 *		на таблицу истории работ;
 *		bUsers_Click - процедура, обработчик нажатия кнопки переключения
 *		на таблицу пользователей;
 *		bPlanesStatus_Click - процедура, обработчик нажатия кнопки открытия
 *		таблицы с вычисленным состоянием технического обслуживнаия
 *		самолётов;
 *		BSave_Click - процедура, обработчик нажатия кнопки сохранения 
 *		внесённых изменений;
 *		BCancel_Click - процедура, обработчик нажатия кнопки отмены
 *		несохраненных изменений;
 *		BAdd_Click - процедура, обработчик нажатия кнопки добавления новой
 *		записи в таблицу;
 *		BDelete_Click - процедура, обработчик нажатия кнопки удаления выбранных
 *		записей из таблицы;
 *		DGrid_DataError - процедура, обработчик автоматических ошибок
 *		источника данных;
 *		FMain_FormClosing - процедура, обработчик выхода из программы;
 *		bLogout_Click - процедура, обработчик нажатия кнопки выхода из
 *		учётной записи.
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

namespace BaseConnection {

	public partial class FMain : Form {
													
													// Создание соединения
		MySqlConnection Connection = QueryUtils.GetConnection();
				
		IDbTable ActiveTable;						// Ныне отображаемая таблица
		BindingSource BSource = new BindingSource();
		
													// Пользователь
		AppUser User;

													// Для возврата на окно логина
		Form LoginForm;
		
													// Таблицы
		MaintTypesTable MaintTypes;
		PlaneModelsTable Models;
		GroupLeaderTable Leaders;
		WorkloadTypesTable Workload;
		TechGroupsTable TechGroups;
		PlanesTable Planes;
		WorksHistoryTable WorksHistory;
		UsersTable Users;

		/*
		 *	Конструктор формы
		 *	
		 *	Формальные параметры:
		 *		user - объект, что определяет пользователя;
		 *		loginForm - ссылка на форму авторизации.
		 */
		public FMain(AppUser user, Form loginForm) {
			InitializeComponent();
													// Передача ссылок
			User = user;
			LoginForm = loginForm;

			this.Text = User.Status + " - " + User.Login + ", id: " + User.Id;

													// Разграничение прав
			if (User.Status == "Гость")
				DisableImportantFunctions();

			else if (User.Status == "Админ") {
				bUsers.Visible = true;
				bUsers.Enabled = true;
				Users = new UsersTable(Connection);
			}


													// Настройка сетки вывода
			DGrid.AutoGenerateColumns = true;
			DGrid.DataSource = BSource;

													// Переопределение ошибки 
													// ввода данных
			DGrid.DataError += new DataGridViewDataErrorEventHandler(DGrid_DataError);

													// Инициализация таблиц
			MaintTypes = new MaintTypesTable(Connection);
			Models = new PlaneModelsTable(Connection);
			Leaders = new GroupLeaderTable(Connection);
			Workload = new WorkloadTypesTable(Connection);
			TechGroups = new TechGroupsTable(Connection);
			Planes = new PlanesTable(Connection);
			WorksHistory = new WorksHistoryTable(Connection);

													// Вывод таблицы по умолчанию
			ActiveTable = MaintTypes;
			ShowTable();

		}

		
													// Переключение доступности 
													//изменения данных
		private void DisableImportantFunctions() {
			DGrid.ReadOnly = true;
			BSave.Enabled = false;
			BCancel.Enabled = false;
			BAdd.Enabled = false;
			BDelete.Enabled = false;
			bPlanesStatus.Enabled = false;
		}
		

													// Вывод таблицы
		private void ShowTable() {
			
			if (ActiveTable != null) {
				
				ActiveTable.Fill();

				BSource.DataSource = ActiveTable.Data.Tables[0];

			} else 
				MessageBox.Show("Выберите таблицу");
		}


													// Переключение таблиц
													// Типы технического обслуживания
		private void bMaintTypes_Click(object sender, EventArgs e) {
			ActiveTable = MaintTypes;
			ShowTable();
		}
													
													// Руководители технических групп
		private void bGroupsLeaders_Click(object sender, EventArgs e) {
			ActiveTable = Leaders;
			ShowTable();
		}

													// Модели самолётов
		private void BPlaneModels_Click(object sender, EventArgs e) {
			ActiveTable = Models;
			ShowTable();
		}

													// Типы объемов работ
		private void bWorkloads_Click(object sender, EventArgs e) {
			ActiveTable = Workload;
			ShowTable();
		}

													// Группы техников
		private void bTechGroups_Click(object sender, EventArgs e) {
			ActiveTable = TechGroups;
			ShowTable();
		}

													// Самолёты
		private void bPlanes_Click(object sender, EventArgs e) {
			ActiveTable = Planes;
			ShowTable();
		}

													// История работ
		private void bWorksHistory_Click(object sender, EventArgs e) {
			ActiveTable = WorksHistory;
			ShowTable();
		}
		
													// Пользователи
		private void bUsers_Click(object sender, EventArgs e) {
			ActiveTable = Users;
			ShowTable();
		}
		

													// Состояние самолётов
		private void bPlanesStatus_Click(object sender, EventArgs e) {
			fPlanesStatus StatusForm = new fPlanesStatus(Connection);
			StatusForm.Show();
		}


													//Действия с таблицами
													// Сохранение
		private void BSave_Click(object sender, EventArgs e) {
			if (ActiveTable.Save())
				ShowTable();
		}

													// Отмена
		private void BCancel_Click(object sender, EventArgs e) {
			ActiveTable.Cancel();
			ShowTable();
		}

													// Добавление строки
		private void BAdd_Click(object sender, EventArgs e) {
			ActiveTable.AddRow();
		}

													// Удаление строки
		private void BDelete_Click(object sender, EventArgs e) {
			
													// По выбранным строкам
			foreach (DataGridViewRow Row in DGrid.SelectedRows)
				if (DGrid.Rows.Contains(Row))
					DGrid.Rows.Remove(Row);

													// По выбранным ячейкам
			foreach (DataGridViewCell Cell in DGrid.SelectedCells)
				DGrid.Rows.Remove(DGrid.Rows[Cell.RowIndex]);

		}

													// Автоматические ошибки 
													// таблицы
		private void DGrid_DataError(object sender, DataGridViewDataErrorEventArgs e) {
			MessageBox.Show("Перепроверьте введённые данные");
			e.ThrowException = false;
		}
		
													// Завершение работы
													// программы
		private void FMain_FormClosing(object sender, FormClosingEventArgs e) {
			Application.Exit();
		}
		
													// Выход из системы	
		private void bLogout_Click(object sender, EventArgs e) {
			LoginForm.Show();
			this.Dispose();
		}

	}

}

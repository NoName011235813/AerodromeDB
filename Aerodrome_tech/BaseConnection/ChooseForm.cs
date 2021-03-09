/*
 *	Форма: ChooseForm
 *	
 *	Язык: C#
 *	Разработал: Штирбу Алин, ТИП - 62
 *	Дата: 04.02.2021г
 *	
 *	Задание: 
 *		На основе переданного набора данных предоставить пользователю
 *		возможность выбрать одну из записей.
 * 
 *	Ожидаемые входные данные:
 *		выбранная строка;
 *		подтверждение действий;
 *		отклонение выбора.
 *		
 *	Переменные, используемые в данной форме:
 *		BSource - посредник между сеткой и набором данных, передаваемом 
 *		пользователем;
 *		ChoosedId - ид выбранной пользователем записи.
 *		
 *	Подпрограммы, используемые в данной форме:
 *		ChooseForm - конструктор класса;
 *		bSubmit_Click - обработчик нажатия на кнопку подтверждения выбора 
 *		записи;
 *		bCancel_Click - обработчик нажатия на кнопку отмены.
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

namespace BaseConnection {

	public partial class ChooseForm : Form {

		BindingSource BSource = new BindingSource();

		public int ChoosedId { get; set; }			// Ид выбранной записи

		/*
		 *	Конструктор класса
		 *	
		 *	Формальный параметр:
		 *		dSet - ссылка на источник данных, что
		 *		должен быть отображен в сетку формы.
		 */
		public ChooseForm(DataTable dSet) {

			InitializeComponent();

			DGrid.DataSource = BSource;
			BSource.DataSource = dSet;

		}

		/*
		 *	Обработчик нажатия кнопки подтверждения
		 *	
		 *	Формальные параметры:
		 *		sender - объект, что вызвал метод;
		 *		e - дополнительные свойства.
		 *		
		 *	Локальная переменная:
		 *		SelectedRowIndex - индекс выбранной пользователем строки.
		 */
		private void bSubmit_Click(object sender, EventArgs e) {

													// Выбранная строка
			int SelectedRowIndex = DGrid.SelectedCells[0].RowIndex;

													// Значение Ид
			ChoosedId = int.Parse(DGrid.Rows[SelectedRowIndex].Cells[0].Value.ToString());
			
													// Передача сообщения о
													// том, что всё прошло успешно
			this.DialogResult = DialogResult.OK;
			//this.Close();
		}

													// Отмена
		private void bCancel_Click(object sender, EventArgs e) {
													// Передача сообщения о 
													// том, что пользователь отменил выбор
			this.DialogResult = DialogResult.Cancel;
			//this.Close();
		}	

	}

}

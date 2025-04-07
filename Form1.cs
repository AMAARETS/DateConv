﻿using System;
using System.Windows.Forms;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DateConv
{
    public partial class Form1 : Form
    {
        private CustomDate customDate; // Add a field to hold the CustomDate instance
        private CustomHebDate customHebDate; // Add a field to hold the CustomHebDate instance        
        private bool isProgrammaticChange = false;
        public Form1()
        {
            InitializeComponent();

            // Attach the event handler to all combo boxes
            comboBoxGregYom.TextChanged += ComboBoxGreg_TextChanged;
            comboBoxGregChodesh.TextChanged += ComboBoxGreg_TextChanged;
            comboBoxGregShana.TextChanged += ComboBoxGreg_TextChanged;
            comboBoxHebYom.TextChanged += ComboBoxHeb_TextChanged;
            comboBoxHebChodesh.TextChanged += ComboBoxHeb_TextChanged;
            comboBoxHebShana.TextChanged += ComboBoxHeb_TextChanged;
            comboBoxHebElef.TextChanged += ComboBoxHeb_TextChanged;
            this.listBoxGreg.DoubleClick += new System.EventHandler(this.listBox_DoubleClick);
            this.listBoxHeb.DoubleClick += new System.EventHandler(this.listBox_DoubleClick);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize the CustomDate instance with the current date
            customDate = new CustomDate();
            customDate.SetDate(DateTime.Now);
            customHebDate = new CustomHebDate();
            customHebDate.setHebDate(customDate.GetDate());
            HagComBoxs();
        }
        private void HagComBoxs()
        {
            isProgrammaticChange = true; // סימון שהשינוי תכנותי

            comboBoxGregYom.SelectedIndex = customDate.Day - 1;
            comboBoxGregChodesh.SelectedIndex = customDate.Month - 1;
            comboBoxGregShana.SelectedIndex = customDate.Year - 1;

            comboBoxHebYom.Text = customHebDate.Day.ToString();
            comboBoxHebChodesh.Text = customHebDate.Month.ToString();
            comboBoxHebShana.Text = customHebDate.Year.ToString();
            comboBoxHebElef.Text = customHebDate.Elef.ToString();

            isProgrammaticChange = false; // חזרה למצב רגיל
        }

        private void ComboBoxGreg_TextChanged(object sender, EventArgs e)
        {
            if (isProgrammaticChange) return; // התעלם משינויים תכנותיים


            if (sender is ComboBox comboBox)
            {
                errorProvider1.SetError(comboBox, string.Empty);
                errorProvider1.SetError(comboBoxGregYom, string.Empty);
                try
                {
                    // Handle changes based on the specific combo box
                    switch (comboBox.Name)
                    {
                        case "comboBoxGregYom":
                            customDate.Day = comboBox.SelectedIndex + 1;
                            break;
                        case "comboBoxGregChodesh":
                            customDate.Month = comboBox.SelectedIndex + 1;
                            break;
                        case "comboBoxGregShana":
                            customDate.Year = comboBox.SelectedIndex + 1;
                            break;
                    }

                    customHebDate.setHebDate(customDate.GetDate());
                }

                catch (ArgumentException ex)
                {
                    if (string.IsNullOrEmpty(comboBox.Text) || !comboBox.Items.Contains(comboBox.Text))
                    {
                        errorProvider1.SetError(comboBox, "ערך לא תקין");
                    }
                    else if (ex.Message.Contains("בחודש זה ישנם רק"))
                    {
                        // Handle the exception as needed
                        errorProvider1.SetError(comboBoxGregYom, ex.Message);
                    }
                    else
                    {
                        errorProvider1.SetError(comboBox, "ערך לא תקין");
                    }
                    return;
                }
                HagComBoxs();
            }
        }

        private void ComboBoxHeb_TextChanged(object sender, EventArgs e)
        {
            if (isProgrammaticChange) return; // התעלם משינויים תכנותיים
            {

                if (sender is ComboBox comboBox)
                {
                    errorProvider1.SetError(comboBox, string.Empty);
                    errorProvider1.SetError(comboBoxHebYom, string.Empty);
                    errorProvider1.SetError(comboBoxHebChodesh, string.Empty);

                    // Handle changes based on the specific combo box

                    switch (comboBox.Name)
                    {
                        case "comboBoxHebYom":
                            customHebDate.Day = comboBox.Text;
                            break;
                        case "comboBoxHebChodesh":
                            customHebDate.Month = comboBox.Text;
                            break;
                        case "comboBoxHebShana":
                            customHebDate.Year = comboBox.Text;
                            break;
                        case "comboBoxHebElef":
                            customHebDate.Elef = comboBox.Text;
                            break;
                    }
                    try
                    {
                        customDate.SetDate(HebDate.convertStrToGregDate(customHebDate.GetHebDate()));
                    }
                    catch (ArgumentException ex)
                    {
                        if (string.IsNullOrEmpty(comboBox.Text) || !comboBox.Items.Contains(comboBox.Text))
                        {
                            errorProvider1.SetError(comboBox, "ערך לא תקין");
                        }
                        else if (ex.Message.Contains("בחודש שנבחר אין יום ל'."))
                        {
                            // Handle the exception as needed
                            errorProvider1.SetError(comboBoxHebYom, ex.Message);
                        }
                        else if (ex.Message.Contains("השנה שנבחרה אינה מעוברת") || ex.Message.Contains("השנה שנבחרה הינה שנה מעוברת יש לבחור אדר א' או ב'"))
                        {
                            // Handle the exception as needed
                            errorProvider1.SetError(comboBoxHebChodesh, ex.Message);
                        }
                        else
                        {
                            errorProvider1.SetError(comboBox, "ערך לא תקין");
                        }
                        return;
                    }
                    HagComBoxs();
                }
            }

        }

        private void buttonShmira_Click(object sender, EventArgs e)
        {
            if (AreAllControlsValid())
            {
                // Save the date
                string gregDate = $"{customDate.Day}/{customDate.Month}/{customDate.Year}";
                string hebDate = customHebDate.GetHebDate();
                listBoxGreg.Items.Add(gregDate);
                listBoxHeb.Items.Add(hebDate);
            }
            else
            {

            }
        }
        private bool AreAllControlsValid()
        {
            foreach (Control control in this.Controls)
            {
                if (!string.IsNullOrEmpty(errorProvider1.GetError(control)))
                {
                    return false; // יש שגיאה באחד הפקדים
                }
            }
            return true; // אין שגיאות
        }
        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem != null)
            {
                // העתקת התוכן של הפריט הנבחר ללוח
                Clipboard.SetText(listBox.SelectedItem.ToString());
            }
        }

        private void dateTimePickerGreg_ValueChanged(object sender, EventArgs e)
        {
            customDate.SetDate(dateTimePickerGreg.Value);
            customHebDate.setHebDate(customDate.GetDate());
            HagComBoxs();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            customDate.SetDate(dateTimePickerGreg.Value);
            customHebDate.setHebDate(customDate.GetDate());
            HagComBoxs();
        }

        private void buttonCopyGreg_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(customDate.GetDate().ToString());
        }

        private void buttonCopyHeb_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(customHebDate.GetHebDate());
        }
    }
}

﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Millionaire
{
    public partial class View : Form
    {
        private Model model;

        private Panel[] moneyPanels;
        private Label[] moneyLevelLabels;
        private Label[] moneyLabels;

        //studio images
        Bitmap[] images = { Properties.Resources.studio1, Properties.Resources.studio2, Properties.Resources.studio3 };
        int currentImage;

        //animation variables
        private int timerTicks;
        private int correctIndex;
        private bool correctAnswer;

        public View()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            Random r = new Random();
            pictureBox.BackgroundImage = images[r.Next(images.Length)];

            moneyPanels = new Panel[15];
            moneyPanels[0] = moneyPanel0;
            moneyPanels[1] = moneyPanel1;
            moneyPanels[2] = moneyPanel2;
            moneyPanels[3] = moneyPanel3;
            moneyPanels[4] = moneyPanel4;
            moneyPanels[5] = moneyPanel5;
            moneyPanels[6] = moneyPanel6;
            moneyPanels[7] = moneyPanel7;
            moneyPanels[8] = moneyPanel8;
            moneyPanels[9] = moneyPanel9;
            moneyPanels[10] = moneyPanel10;
            moneyPanels[11] = moneyPanel11;
            moneyPanels[12] = moneyPanel12;
            moneyPanels[13] = moneyPanel13;
            moneyPanels[14] = moneyPanel14;

            moneyLevelLabels = new Label[15];
            moneyLevelLabels[0] = moneyLevelLabel0;
            moneyLevelLabels[1] = moneyLevelLabel1;
            moneyLevelLabels[2] = moneyLevelLabel2;
            moneyLevelLabels[3] = moneyLevelLabel3;
            moneyLevelLabels[4] = moneyLevelLabel4;
            moneyLevelLabels[5] = moneyLevelLabel5;
            moneyLevelLabels[6] = moneyLevelLabel6;
            moneyLevelLabels[7] = moneyLevelLabel7;
            moneyLevelLabels[8] = moneyLevelLabel8;
            moneyLevelLabels[9] = moneyLevelLabel9;
            moneyLevelLabels[10] = moneyLevelLabel10;
            moneyLevelLabels[11] = moneyLevelLabel11;
            moneyLevelLabels[12] = moneyLevelLabel12;
            moneyLevelLabels[13] = moneyLevelLabel13;
            moneyLevelLabels[14] = moneyLevelLabel14;

            moneyLabels = new Label[15];
            moneyLabels[0] = moneyLabel0;
            moneyLabels[1] = moneyLabel1;
            moneyLabels[2] = moneyLabel2;
            moneyLabels[3] = moneyLabel3;
            moneyLabels[4] = moneyLabel4;
            moneyLabels[5] = moneyLabel5;
            moneyLabels[6] = moneyLabel6;
            moneyLabels[7] = moneyLabel7;
            moneyLabels[8] = moneyLabel8;
            moneyLabels[9] = moneyLabel9;
            moneyLabels[10] = moneyLabel10;
            moneyLabels[11] = moneyLabel11;
            moneyLabels[12] = moneyLabel12;
            moneyLabels[13] = moneyLabel13;
            moneyLabels[14] = moneyLabel14;
        }

        private void newGame_btn_Click(object sender, EventArgs e)
        {
            newGame();
        }

        private void info_btn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format("Кој сака да биде милионер ?\nИзработија:\nДавид Симеоновски\nНикола Танев"), "Кој сака да биде милионер ?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exit_btn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void newGame()
        {
            //Starts a new game
            model = new Model();
            updateView();
            playPanel.Show();
            imageTimer.Enabled = true;

            //reset the used jokers from previous game
            fifty_joker.Enabled = true;
            fifty_joker.BackgroundImage = Properties.Resources._5050;
            audience_joker.Enabled = true;
            audience_joker.BackgroundImage = Properties.Resources.audience;
            phone_joker.Enabled = true;
            phone_joker.BackgroundImage = Properties.Resources.phone;
            switch_joker.Enabled = true;
            switch_joker.BackgroundImage = Properties.Resources._switch;
        }

        private void updateView()
        {
            //updates all elements in the view.

            //update question
            enableAllButtons();
            Question q = model.currentQuestion;
            questionLabel.Text = q.question;
            answerTextA.Text = q.answer[0];
            answerTextB.Text = q.answer[1];
            answerTextV.Text = q.answer[2];
            answerTextG.Text = q.answer[3];

            //update jokers
            if (model.fifty_spent) fifty_joker.Enabled = false;
            if (model.audience_spent) audience_joker.Enabled = false;
            if (model.phone_spent) phone_joker.Enabled = false;
            if (model.switch_spent) switch_joker.Enabled = false;

            //update money panel
            updateMoneyPanel();

            //check for joker 50-50
            if (model.fifty_active)
            {
                //joker 50-50 is active
                for(int i = 0; i<4; i++)
                {
                    //disable answers
                    if (model.fifty_wrong1 == i || model.fifty_wrong2 == i) disableButton(i);
                }
            }

            //update level
            levelLabel.Text = "" + (model.level + 1);
        }

        private void updateMoneyPanel()
        {
            for (int i = 0; i < 15; i++)
            {
                //reset all fields to default
                moneyPanels[i].BackColor = Color.FromArgb(35, 31, 32);
                moneyLabels[i].BackColor = Color.FromArgb(35, 31, 32);
                moneyLevelLabels[i].BackColor = Color.FromArgb(35, 31, 32);
                moneyLevelLabels[i].ForeColor = Color.FromArgb(248, 155, 28);
                moneyLabels[i].ForeColor = Color.FromArgb(248, 155, 28);
            }
            moneyLevelLabels[4].ForeColor = Color.White;
            moneyLabels[4].ForeColor = Color.White;
            moneyLevelLabels[9].ForeColor = Color.White;
            moneyLabels[9].ForeColor = Color.White;
            moneyLevelLabels[14].ForeColor = Color.White;
            moneyLabels[14].ForeColor = Color.White;

            moneyPanels[model.level].BackColor = Color.FromArgb(248, 155, 28);
            moneyLabels[model.level].BackColor = Color.FromArgb(248, 155, 28);
            moneyLevelLabels[model.level].BackColor = Color.FromArgb(248, 155, 28);
            if ((model.level + 1) % 5 != 0) // checks if the current level is 5, 10 or 15
            {
                moneyLevelLabels[model.level].ForeColor = Color.Black;
                moneyLabels[model.level].ForeColor = Color.Black;
            }
        }

        private Label getLetterLabel(int index)
        {
            //returns the letter label for the given index
            if(index == 0) return labelA;
            else if(index == 1) return labelB;
            else if(index == 2) return labelV;
            else return labelG;
        }

        private Label getAnswerTextLabel(int index)
        {
            //returns the answerText label for the given index
            if (index == 0) return answerTextA;
            else if (index == 1) return answerTextB;
            else if (index == 2) return answerTextV;
            else return answerTextG;
        }

        private Panel getPanel(int index)
        {
            //returns the button panel for the given index
            if (index == 0) return panelA;
            else if (index == 1) return panelB;
            else if (index == 2) return panelV;
            else return panelG;
        }

        private void disableButton(int index)
        {
            //disables the button with the given index (from 0 to 3 inclusive)
            if(index == 0 || index == 2) getPanel(index).BackgroundImage = Properties.Resources.leftAnswerDisabled;
            else getPanel(index).BackgroundImage = Properties.Resources.rightAnswerDisabled;
            getLetterLabel(index).Text = "";
            getAnswerTextLabel(index).Text = "";
            getPanel(index).Enabled = false;
        }

        private void enableAllButtons()
        {
            //enables all answers
            panelA.Enabled = panelB.Enabled = panelV.Enabled = panelG.Enabled = true;
            panelA.BackgroundImage = panelV.BackgroundImage = Properties.Resources.leftAnswer;
            panelB.BackgroundImage = panelG.BackgroundImage = Properties.Resources.rightAnswer;
            labelA.Text = "А:";
            labelB.Text = "Б:";
            labelV.Text = "В:";
            labelG.Text = "Г:";
        }

        private void changeBackgroundNormal(int index)
        {
            //change backround of answer to normal (unselected)
            if (index == 0 || index == 2) getPanel(index).BackgroundImage = Properties.Resources.leftAnswer;
            else getPanel(index).BackgroundImage = Properties.Resources.rightAnswer;
            getLetterLabel(index).BackColor = Color.FromArgb(35, 31, 32);
            getLetterLabel(index).ForeColor = Color.FromArgb(248, 155, 28); //orange;
            getAnswerTextLabel(index).BackColor = Color.FromArgb(35, 31, 32);
            getAnswerTextLabel(index).ForeColor = Color.White;
        }

        private void changeBackgroundSelected(int index)
        {
            //change backround of answer to selected
            if (index == 0 || index == 2) getPanel(index).BackgroundImage = Properties.Resources.leftAnswerSelect;
            else getPanel(index).BackgroundImage = Properties.Resources.rightAnswerSelect;
            getLetterLabel(index).BackColor = Color.FromArgb(248, 155, 28);
            getLetterLabel(index).ForeColor = Color.White;
            getAnswerTextLabel(index).BackColor = Color.FromArgb(248, 155, 28);
            getAnswerTextLabel(index).ForeColor = Color.Black;
        }

        private void changeBackgroundCorrect(int index)
        {
            //change backround of answer to correct
            if (index == 0 || index == 2) getPanel(index).BackgroundImage = Properties.Resources.leftAnswerCorrect;
            else getPanel(index).BackgroundImage = Properties.Resources.rightAnswerCorrect;
            getLetterLabel(index).BackColor = Color.FromArgb(83, 187, 97);
            getLetterLabel(index).ForeColor = Color.White;
            getAnswerTextLabel(index).BackColor = Color.FromArgb(83, 187, 97);
            getAnswerTextLabel(index).ForeColor = Color.Black;
        }

        private void answer0_Click(object sender, EventArgs e)
        {
            tryAnswer(0);
        }

        private void answer1_Click(object sender, EventArgs e)
        {
            tryAnswer(1);
        }

        private void answer2_Click(object sender, EventArgs e)
        {
            tryAnswer(2);
        }

        private void answer3_Click(object sender, EventArgs e)
        {
            tryAnswer(3);
        }

        private void showCorrectAnswereMessage(int price, int level)
        {
            CorrectAnswerMessageForm form = new CorrectAnswerMessageForm();
            form.showMessage(price, level);
            form.ShowDialog();
        }

        private void animateCorrect(int index)
        {
            correctIndex = index;
            correctAnswer = true;
            timerTicks = 9; //needs to be set to odd number
            animationTimer.Start(); //starts the animation
        }

        private void animateWrong(int index)
        {
            correctIndex = index;
            correctAnswer = false;
            timerTicks = 5; //needs to be set to odd number
            animationTimer.Start(); //starts the animation
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //animates the correct answer with green flickering
            if(timerTicks % 2 == 1)
            {
                //green flicker
                changeBackgroundCorrect(correctIndex);
            }
            else
            {
                //default color flicker
                if(correctAnswer) changeBackgroundSelected(correctIndex);
                else changeBackgroundNormal(correctIndex);       
            }

            timerTicks--;
            if (timerTicks == 0) animationTimer.Stop();
        }

        public void tryAnswer(int index)
        {
            changeBackgroundSelected(index);
            string answer = model.currentQuestion.answer[index];

            if (finalAnswer(answer))
            {
                if (model.tryAnswer(index))
                {
                    //correct answer
                    animateCorrect(index);
                    showCorrectAnswereMessage(model.getMoney(false), model.level);
                    updateView();
                }
                else
                {
                    //wrong answer
                    animateWrong(model.correct);

                    bool newGameBool = false;
                    if (wrongAnswerMessage(answer, model.currentQuestion.answer[model.correct]))
                    {
                        //player wants new game
                        newGameBool = true;
                        
                    }

                    //reset the correct button first
                    changeBackgroundNormal(model.correct);
                    
                    if(newGameBool) newGame();
                    else playPanel.Hide();
                }
            }
            changeBackgroundNormal(index);
        }

        private bool wrongAnswerMessage(string answer, string correct)
        {
            return MessageBox.Show(string.Format("Одговорот „{0}“ е грешен! Точниот одговор е „{1}“.\n\nВие освоивте {2} денари.\n\nНова игра?", answer, correct, model.getMoney(true)), "Грешен одговор", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        private bool serrenderAnswerMessage(string correct)
        {
            return MessageBox.Show(
                string.Format("Точниот одговор на прашањето е „{0}“.\n\nВие освоивте {1} денари.\n\nНова игра ?", correct, model.getMoney(false)), 
                "Се откажавте !", 
                MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        private bool finalAnswer(string answer)
        {
            return MessageBox.Show(string.Format("Дали „{0}“ е вашиот конечен одговор ?", answer),"Конечен одговор ?", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        private void surrender_btn_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Дали навистина сакате да се откажете ?", 
                "Се откажувате ?", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Information) == DialogResult.Yes)
            {

                //TODO: display final score
                if (serrenderAnswerMessage(model.currentQuestion.answer[model.correct]))
                {
                    //playere wants new game
                    newGame();
                }
                else
                {
                    //player doesn't want new game
                    playPanel.Hide();
                }
            }

        }

        private void fifty_joker_Click(object sender, EventArgs e)
        {
            fifty_joker.BackgroundImage = Properties.Resources._5050inuse;
            timer5050.Start();
            model.joker_fifty();
            updateView();
        }

        private void audience_joker_Click(object sender, EventArgs e)
        {
            audience_joker.BackgroundImage = Properties.Resources.audienceinuse;
            timerAudience.Start();

            int[] votes = model.joker_audience();

            Audience view = new Audience(votes);
            view.ShowDialog();
            updateView();
        }

        private void phone_joker_Click(object sender, EventArgs e)
        {
            phone_joker.BackgroundImage = Properties.Resources.phoneinuse;            
            timerPhone.Start();

            int[] phone = model.joker_phone();
            Phone view = new Phone(phone[0], model.currentQuestion.answer[phone[0]], phone[1]);
            view.ShowDialog();
            updateView();
        }

        private void switch_joker_Click(object sender, EventArgs e)
        {
            switch_joker.BackgroundImage = Properties.Resources.switchinuse;
            timerSwitch.Start();
            model.joker_switch();
            updateView();
        }

        private void timer5050_Tick(object sender, EventArgs e)
        {
            fifty_joker.BackgroundImage = Properties.Resources._5050used;
            timer5050.Stop();
        }

        private void timerPhone_Tick(object sender, EventArgs e)
        {
            phone_joker.BackgroundImage = Properties.Resources.phoneused;
            timerPhone.Stop();
        }

        private void timerAudience_Tick(object sender, EventArgs e)
        {
            audience_joker.BackgroundImage = Properties.Resources.audienceused;
            timerAudience.Stop();
        }

        private void timerSwitch_Tick(object sender, EventArgs e)
        {
            switch_joker.BackgroundImage = Properties.Resources.switchused;
            timerSwitch.Stop();
        }

        private void imageTimer_Tick(object sender, EventArgs e)
        {
            //changes the current image
            Random r = new Random();
            int rand = r.Next(images.Length - 1);
            if (rand >= currentImage) rand++;
            currentImage = rand;
            pictureBox.BackgroundImage = images[rand];
        }

        private void panelA_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X >= 55 && e.X <= 379) tryAnswer(0);
        }

        private void panelB_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X >= 14 && e.X <= 337) tryAnswer(1);
        }

        private void panelV_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X >= 55 && e.X <= 379) tryAnswer(2);
        }

        private void panelG_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X >= 14 && e.X <= 337) tryAnswer(3);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quizz
{
    public partial class Form1 : Form
    {
        // declaring my variables
        private readonly Random random = new Random();
        private int operand1;
        private int operand2;
        private char operatorChar;
        private int score;
        private int currentProblem;
        private const int TotalProblems = 10;
        public Form1()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.Form1_Load);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerateProblem();
        }

        
        private void GenerateProblem()
        {
            // gets a random operator
            operatorChar = GetRandomOperator();

            if (operatorChar == '×')
            {
                operand1 = GetRandomNumber(1, 12);
                operand2 = GetRandomNumber(1, 12);
            }
            else if (operatorChar == '÷')
            {
                operand2 = GetRandomNumber(1, 12);
                operand1 = operand2 * GetRandomNumber(1, 12);
            }
            else
            {
                operand1 = GetRandomNumber(1, 50);
                operand2 = GetRandomNumber(1, 50);
            }

            //here the labels will update to actually show the current problem
            operand1Label.Text = operand1.ToString();
            operand2Label.Text = operand2.ToString();
            operatorLabel.Text = operatorChar.ToString();
            answerTextBox.Text = "";

            label2.Text = $"Problem {currentProblem} / {TotalProblems}";
        }

        private char GetRandomOperator()
        {
            string operators = "+-×÷";
            int randomIndex = random.Next(operators.Length);
            return operators[randomIndex];
        }

        private int GetRandomNumber(int min, int max)
        {
            return random.Next(min, max + 1);
        }

        //this button checks the answer provided and gives feedback based on it
        private void validateButton_Click(object sender, EventArgs e)
        {
            int correctAnswer = CalculateCorrectAnswer();

            //check if answer provided is actually an int and equal to the already calculated correct answer
            if (int.TryParse(answerTextBox.Text, out int userAnswer) && userAnswer == correctAnswer)
            {
                score++;
                MessageBox.Show("Correct!");
            }
            else
            {
                MessageBox.Show($"Wrong! The correct answer is {correctAnswer}");
            }

            currentProblem++;
            if (currentProblem <= TotalProblems)
            {
                GenerateProblem();
            }
            else
            {
                MessageBox.Show($"Quiz Completed! Final Score: {score}/{TotalProblems}");
                RestartQuiz();
            }
        }

        private int CalculateCorrectAnswer()
        {
            switch (operatorChar)
            {
                case '+':
                    return operand1 + operand2;
                case '-':
                    return operand1 - operand2;
                case '×':
                    return operand1 * operand2;
                case '÷':
                    return operand1 / operand2;
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }

        private void RestartQuiz()
        {
            var result = MessageBox.Show("Do you want to restart the quiz?", "Quiz Completed", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                score = 0;
                currentProblem = 1;
                GenerateProblem();
            }
            else if (result == DialogResult.No)
            {
                Application.Exit();
            }
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            RestartQuiz();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //this function checks the answer input as its being typed to prevent anything other than numbers from being typed
        private void CheckAnswer(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(answerTextBox.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter a number!");
                answerTextBox.Text = answerTextBox.Text.Remove(answerTextBox.Text.Length - 1);
            }
        }
    }
}

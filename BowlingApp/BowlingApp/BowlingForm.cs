﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BowlingApp
{
    public partial class BowlingForm : Form
    {
        public int currentFrame;
        public bool[] spareFrames;
        public bool[] strikeFrames;
        public Dictionary<int, List<int>> frameScores;
        
        public BowlingForm()
        {
            InitializeComponent();

            currentFrame = 1;
            spareFrames = new bool[11];
            strikeFrames = new bool[11];
            frameScores = new Dictionary<int, List<int>>();

            FrameLabel.Text = $"Frame {currentFrame}";
        }

        public void ScoreButton_Click(object sender, EventArgs e)
        {
            if (!frameScores.ContainsKey(currentFrame)) 
                frameScores.Add(currentFrame, new List<int>());

            if(NewScoreIsValid())
            {
                int newScore = int.Parse(scoreTextBox.Text);
                frameScores[currentFrame].Add(newScore);

                UpdatePreviousStrikesAndSpares(newScore);
                CheckStrikeOrSpare(newScore);
                CheckEndOfFrame();

                ErrorLabel.Text = null;
            }

            scoreTextBox.Text = null;
        }

        private bool NewScoreIsValid()
        {
            try
            {
                int newScore = int.Parse(scoreTextBox.Text);

                if (newScore < 0 || newScore > 10)
                {
                    ErrorLabel.Text = "Invalid input, please enter a number between 0 and 10";
                    return false;
                }
                else
                {
                    int thisFrameScore = frameScores[currentFrame].Sum();
                    if (currentFrame < 10)
                    {
                        if (thisFrameScore + newScore > 10)
                        {
                            ErrorLabel.Text = "Invalid input, can't score over 10 in one frame!";
                            return false;
                        }
                    }
                    else
                    {
                        if (thisFrameScore + newScore > 10 && frameScores[currentFrame].Count == 1 && !strikeFrames[currentFrame])
                        {
                            ErrorLabel.Text = "Invalid input, can't score over 10 without a strike first!";
                            return false;
                        }
                        else if (frameScores[currentFrame].Count == 2 && !spareFrames[currentFrame])
                        {
                            int lastBowl = frameScores[currentFrame].Last();
                            if (lastBowl != 10 && lastBowl + newScore > 10)
                            {
                                ErrorLabel.Text = "Invalid input, can't score over 10!";
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                ErrorLabel.Text = "Invalid input, please enter a valid number";
                return false;
            }

            return true;
        }

        private void UpdatePreviousStrikesAndSpares(int newScore)
        {
            if (currentFrame > 1 && strikeFrames[currentFrame - 1] && frameScores[currentFrame].Count <= 2)
            {
                frameScores[currentFrame - 1].Add(newScore);

                if (currentFrame > 2 && strikeFrames[currentFrame - 2] && frameScores[currentFrame].Count <= 1)
                    frameScores[currentFrame - 2].Add(newScore);
            }

            if (currentFrame > 1 && spareFrames[currentFrame - 1] && frameScores[currentFrame].Count == 1)
                frameScores[currentFrame - 1].Add(newScore);
        }

        private void CheckStrikeOrSpare(int newScore)
        {
            if (frameScores[currentFrame].Count == 2 && frameScores[currentFrame].Sum() == 10)
            {
                spareFrames[currentFrame] = true;
            }
            else if (newScore == 10)
            {
                strikeFrames[currentFrame] = true;
            }
        }

        private void CheckEndOfFrame()
        {
            if (currentFrame < 10 && (strikeFrames[currentFrame] || frameScores[currentFrame].Count == 2))
            {
                NewFrame();
            }
            else
            {
                if (frameScores[currentFrame].Count == 2 && !strikeFrames[currentFrame] && !spareFrames[currentFrame])
                {
                    CompleteGame();
                }
                else if (frameScores[currentFrame].Count == 3)
                {
                    CompleteGame();
                }
            }
        }

        private void NewFrame()
        {
            DisplayUpdatedScores();
            currentFrame++;
            FrameLabel.Text = $"Frame {currentFrame}";
        }

        private void CompleteGame()
        {
            DisplayUpdatedScores();

            scoreTextBox.Visible = false;
            ScoreButton.Visible = false;

            string completeText = "Game Complete!";
            if (TotalScoreLabel.Text == "0") completeText += "...at least you finished!";
            FrameLabel.Text = completeText;
        }

        private void DisplayUpdatedScores()
        {
            //for the record I would never do this but I have no idea how to dynamically assign labels in Windows Forms
            f1Score.Text = frameScores.ContainsKey(1) ? frameScores[1].Sum().ToString() : null;
            f2Score.Text = frameScores.ContainsKey(2) ? (frameScores[2].Sum() + int.Parse(f1Score.Text)).ToString() : null;
            f3Score.Text = frameScores.ContainsKey(3) ? (frameScores[3].Sum() + int.Parse(f2Score.Text)).ToString() : null;
            f4Score.Text = frameScores.ContainsKey(4) ? (frameScores[4].Sum() + int.Parse(f3Score.Text)).ToString() : null;
            f5Score.Text = frameScores.ContainsKey(5) ? (frameScores[5].Sum() + int.Parse(f4Score.Text)).ToString() : null;
            f6Score.Text = frameScores.ContainsKey(6) ? (frameScores[6].Sum() + int.Parse(f5Score.Text)).ToString() : null;
            f7Score.Text = frameScores.ContainsKey(7) ? (frameScores[7].Sum() + int.Parse(f6Score.Text)).ToString() : null;
            f8Score.Text = frameScores.ContainsKey(8) ? (frameScores[8].Sum() + int.Parse(f7Score.Text)).ToString() : null;
            f9Score.Text = frameScores.ContainsKey(9) ? (frameScores[9].Sum() + int.Parse(f8Score.Text)).ToString() : null;
            f10Score.Text = frameScores.ContainsKey(10) ? (frameScores[10].Sum() + int.Parse(f9Score.Text)).ToString() : null;

            int score = 0;
            foreach(List<int> scores in frameScores.Values)
            {
                score += scores.Sum();
            }

            TotalScoreLabel.Text = score.ToString();
        }
    }
}
using Xunit;
using BowlingApp;
using System.Linq;

namespace TestBowlingApp
{
    public class TestBowlingForm
    {
        BowlingForm bowlingForm = new BowlingForm();

        [Fact]
        public void TestNegativeInput()
        {
            bowlingForm.scoreTextBox.Text = "-1";
            bowlingForm.ScoreButton_Click(bowlingForm, null);

            Assert.Equal("Invalid input, please enter a number between 0 and 10", bowlingForm.ErrorLabel.Text);
            Assert.Empty(bowlingForm.frameScores[1]);
        }

        [Fact]
        public void TestNonNumberInput()
        {
            bowlingForm.scoreTextBox.Text = "asd";
            bowlingForm.ScoreButton_Click(bowlingForm, null);

            Assert.Equal("Invalid input, please enter a valid number", bowlingForm.ErrorLabel.Text);
            Assert.Empty(bowlingForm.frameScores[1]);
        }

        [Fact]
        public void TestNumberOverTenInput()
        {
            bowlingForm.scoreTextBox.Text = "11";
            bowlingForm.ScoreButton_Click(bowlingForm, null);

            Assert.Equal("Invalid input, please enter a number between 0 and 10", bowlingForm.ErrorLabel.Text);
            Assert.Empty(bowlingForm.frameScores[1]);
        }

        [Fact]
        public void TestFrameOverTenInput()
        {
            bowlingForm.scoreTextBox.Text = "5";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            bowlingForm.scoreTextBox.Text = "6";
            bowlingForm.ScoreButton_Click(bowlingForm, null);

            Assert.Equal("Invalid input, can't score over 10 in one frame!", bowlingForm.ErrorLabel.Text);
            Assert.Equal(1, bowlingForm.frameScores[1].Count);
        }

        [Fact]
        public void TestStrikeIncrementsFrame()
        {
            bowlingForm.scoreTextBox.Text = "10";
            bowlingForm.ScoreButton_Click(bowlingForm, null);

            Assert.Equal(2, bowlingForm.currentFrame);
        }

        [Fact]
        public void TestEmptyThenStrikeCountsAsSpare()
        {
            bowlingForm.scoreTextBox.Text = "0";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            bowlingForm.scoreTextBox.Text = "10";
            bowlingForm.ScoreButton_Click(bowlingForm, null);

            bowlingForm.scoreTextBox.Text = "5";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            bowlingForm.scoreTextBox.Text = "5"; //should not count this one
            bowlingForm.ScoreButton_Click(bowlingForm, null);

            Assert.Equal(15, bowlingForm.frameScores[1].Sum());
            Assert.True(bowlingForm.spareFrames[1]);
            Assert.False(bowlingForm.strikeFrames[1]);
        }

        [Fact]
        public void TestStrikeGetsNextTwoBowls()
        {
            bowlingForm.scoreTextBox.Text = "10";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            Assert.Equal(10, bowlingForm.frameScores[1].Sum());

            bowlingForm.scoreTextBox.Text = "10";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            Assert.Equal(20, bowlingForm.frameScores[1].Sum());

            bowlingForm.scoreTextBox.Text = "10";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            Assert.Equal(30, bowlingForm.frameScores[1].Sum());

            bowlingForm.scoreTextBox.Text = "10"; //first strike no longer relevant
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            Assert.Equal(30, bowlingForm.frameScores[1].Sum());
        }

        [Fact]
        public void TestSpareGetsNextBowl()
        {
            bowlingForm.scoreTextBox.Text = "5";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            bowlingForm.scoreTextBox.Text = "5";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            Assert.Equal(10, bowlingForm.frameScores[1].Sum());

            bowlingForm.scoreTextBox.Text = "5";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            Assert.Equal(15, bowlingForm.frameScores[1].Sum());

            bowlingForm.scoreTextBox.Text = "5"; //spare no longer relevant
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            Assert.Equal(15, bowlingForm.frameScores[1].Sum());
        }

        [Fact]
        public void TestLastFrameThreeStrikes()
        {
            for (int i = 1; i <= 9; i++)
            {
                bowlingForm.scoreTextBox.Text = "10";
                bowlingForm.ScoreButton_Click(bowlingForm, null);
            }
            Assert.Equal(10, bowlingForm.currentFrame);

            bowlingForm.scoreTextBox.Text = "10";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            bowlingForm.scoreTextBox.Text = "10";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            bowlingForm.scoreTextBox.Text = "10";
            bowlingForm.ScoreButton_Click(bowlingForm, null);

            Assert.Equal("270", bowlingForm.f9Score.Text);
            Assert.Equal("300", bowlingForm.f10Score.Text);
            Assert.Equal(30, bowlingForm.frameScores[10].Sum());
            Assert.Equal("Game Complete!", bowlingForm.FrameLabel.Text);
        }

        [Fact]
        public void TestLastFrameTwoStrikesOpen()
        {
            for (int i = 1; i <= 9; i++)
            {
                bowlingForm.scoreTextBox.Text = "10";
                bowlingForm.ScoreButton_Click(bowlingForm, null);
            }
            Assert.Equal(10, bowlingForm.currentFrame);

            bowlingForm.scoreTextBox.Text = "10";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            bowlingForm.scoreTextBox.Text = "10";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            bowlingForm.scoreTextBox.Text = "9";
            bowlingForm.ScoreButton_Click(bowlingForm, null);

            Assert.Equal("270", bowlingForm.f9Score.Text);
            Assert.Equal("299", bowlingForm.f10Score.Text);
            Assert.Equal(29, bowlingForm.frameScores[10].Sum());
            Assert.Equal("Game Complete!", bowlingForm.FrameLabel.Text);
        }

        [Fact]
        public void TestLastFrameStrikeThenOpen()
        {
            for (int i = 1; i <= 9; i++)
            {
                bowlingForm.scoreTextBox.Text = "10";
                bowlingForm.ScoreButton_Click(bowlingForm, null);
            }
            Assert.Equal(10, bowlingForm.currentFrame);

            bowlingForm.scoreTextBox.Text = "10";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            bowlingForm.scoreTextBox.Text = "4";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            bowlingForm.scoreTextBox.Text = "4";
            bowlingForm.ScoreButton_Click(bowlingForm, null);

            Assert.Equal("264", bowlingForm.f9Score.Text);
            Assert.Equal("282", bowlingForm.f10Score.Text);
            Assert.Equal(18, bowlingForm.frameScores[10].Sum());
            Assert.Equal("Game Complete!", bowlingForm.FrameLabel.Text);
        }

        [Fact]
        public void TestLastFrameStrikeNextTwoInvalid()
        {
            for (int i = 1; i <= 9; i++)
            {
                bowlingForm.scoreTextBox.Text = "10";
                bowlingForm.ScoreButton_Click(bowlingForm, null);
            }
            Assert.Equal(10, bowlingForm.currentFrame);

            bowlingForm.scoreTextBox.Text = "10";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            bowlingForm.scoreTextBox.Text = "5";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            bowlingForm.scoreTextBox.Text = "6";
            bowlingForm.ScoreButton_Click(bowlingForm, null);

            Assert.Equal(15, bowlingForm.frameScores[10].Sum());
            Assert.Equal("Invalid input, can't score over 10!", bowlingForm.ErrorLabel.Text);
        }

        [Fact]
        public void TestLastFrameSpareThenAnything()
        {
            for (int i = 1; i <= 9; i++)
            {
                bowlingForm.scoreTextBox.Text = "10";
                bowlingForm.ScoreButton_Click(bowlingForm, null);
            }
            Assert.Equal(10, bowlingForm.currentFrame);

            bowlingForm.scoreTextBox.Text = "5";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            bowlingForm.scoreTextBox.Text = "5";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            bowlingForm.scoreTextBox.Text = "5";
            bowlingForm.ScoreButton_Click(bowlingForm, null);

            Assert.Equal("255", bowlingForm.f9Score.Text);
            Assert.Equal("270", bowlingForm.f10Score.Text);
            Assert.Equal(15, bowlingForm.frameScores[10].Sum());
            Assert.Equal("Game Complete!", bowlingForm.FrameLabel.Text);
        }

        [Fact]
        public void TestLastFrameOpen()
        {
            for (int i = 1; i <= 9; i++)
            {
                bowlingForm.scoreTextBox.Text = "10";
                bowlingForm.ScoreButton_Click(bowlingForm, null);
            }
            Assert.Equal(10, bowlingForm.currentFrame);

            bowlingForm.scoreTextBox.Text = "4";
            bowlingForm.ScoreButton_Click(bowlingForm, null);
            bowlingForm.scoreTextBox.Text = "4";
            bowlingForm.ScoreButton_Click(bowlingForm, null);

            Assert.Equal("252", bowlingForm.f9Score.Text);
            Assert.Equal("260", bowlingForm.f10Score.Text);
            Assert.Equal(8, bowlingForm.frameScores[10].Sum());
            Assert.Equal("Game Complete!", bowlingForm.FrameLabel.Text);
        }

        [Fact]
        public void TestWorstBowlerEver()
        {
            for (int i = 1; i <= 20; i++)
            {
                bowlingForm.scoreTextBox.Text = "0";
                bowlingForm.ScoreButton_Click(bowlingForm, null);
            }

            Assert.Equal("Game Complete!...at least you finished!", bowlingForm.FrameLabel.Text);
        }
    }
}

using System;
namespace CeltiberianTest.API.Models
{
	public class LetterInfo
    {
		public char Letter { get; set; }
		public int Amount { get; set; }

		public LetterInfo()
		{
            Letter = default;
            Amount = default;
		}

		public LetterInfo(char letter, int amount) {
			Letter = letter;
			Amount = amount;
		}
	}
}
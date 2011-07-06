using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HexDumper
{
	public partial class Form1 : Form
	{
		List<string> hexFile = new List<string> { };
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			string filepath;
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				filepath = openFileDialog1.FileName;
				using (StreamReader reader = new StreamReader(filepath))
				{
					int linecount = 0;
					int position = 0;
					while (!reader.EndOfStream)
					{
						char[] buffer = new char[16];
						int charactersRead = reader.ReadBlock(buffer, 0, 16);
						// Add a new line to the list, starting with the begining part, i.e. 0000: 
						// Note this first part is in hex, so it'll show 0010: for the 2nd line, even though
						// it read 16 characters in. 10 is 16 in hex.
						hexFile.Add(String.Format("{0:x4}", position) + ": ");
						position += charactersRead;
						// Add the actual hex values
						for (int i = 0; i < 16; i++)
						{
							// if it's not at the end of the batch of 16 characters, print the hex formated byte
							if (i < charactersRead)
							{
								string hex = String.Format("{0:x2}", (byte)buffer[i]);
								hexFile[linecount] += hex + " ";
							}
							// If it is at the end of the batch, add a space.
							else
							{
								hexFile[linecount] += " ";
							}
							// If halfway through, add -- on the line.
							if (i == 7)
							{
								hexFile[linecount] += "-- ";
							}
							// if hex is out of the a certain range which represents characters, just print a period.
							// (i think that's what this is doing)
							if (buffer[i] < 32 || buffer[i] > 250)
							{
								buffer[i] = '.';
							}
						}
						// adds the readable text to the end of the line
						string bufferContents = new string(buffer);
						hexFile[linecount] += (" " + bufferContents.Substring(0, charactersRead));
						// Increment the line count.
						linecount++;
					}
				}
				button2.Enabled = true;
				textBox1.Clear();
				textBox1.Lines = hexFile.ToArray<string>();
				/*	
				   foreach (string line in hexFile)
							{
								Console.WriteLine(line);
							}
				 */
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			string filepath;
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				filepath = saveFileDialog1.FileName;
				using (StreamWriter writer = new StreamWriter(filepath))
				{
					foreach (string line in hexFile)
					{
						writer.WriteLine(line);
					}
				}
				System.Diagnostics.Process.Start("notepad.exe", filepath);
			}
		}
	}
}

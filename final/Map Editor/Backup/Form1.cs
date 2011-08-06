using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace WindowsFormsApplication2
{
    public struct IconInfo
    {
        public bool fIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "Map Editor";
            NormalCursor = this.Cursor;
            currentMap = new Map();
            currentSelected = new List<int>();
            ColorBox.Items.Add(Color.Black);
            ColorBox.Items.Add(Color.Blue);
            ColorBox.Items.Add(Color.Brown);
            ColorBox.Items.Add(Color.Cyan);
            ColorBox.Items.Add(Color.Gold);
            ColorBox.Items.Add(Color.Gray);
            ColorBox.Items.Add(Color.Green);
            ColorBox.Items.Add(Color.Maroon);
            ColorBox.Items.Add(Color.Navy);
            ColorBox.Items.Add(Color.Orange);
            ColorBox.Items.Add(Color.Purple);
            ColorBox.Items.Add(Color.Red);
            ColorBox.Items.Add(Color.Silver);
            ColorBox.Items.Add(Color.Teal);
            ColorBox.Items.Add(Color.Turquoise);
            ColorBox.Items.Add(Color.Yellow);
            ColorBox.SelectedIndex = 0;
            ColorPanel.BackColor = (Color)ColorBox.SelectedItem;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
        {
            IconInfo tmp = new IconInfo();
            GetIconInfo(bmp.GetHicon(), ref tmp);
            tmp.xHotspot = xHotSpot;
            tmp.yHotspot = yHotSpot;
            tmp.fIcon = false;
            return new Cursor(CreateIconIndirect(ref tmp));
        }

        private void MapPanel_MouseEnter(object sender, EventArgs e)
        {
            if (ObjectButton.Enabled == false)
            {
                Bitmap bitmap = new Bitmap((int)(ObjectSizeNumber.Value * XNumber.Value), (int)(ObjectSizeNumber.Value * YNumber.Value));
                Graphics g = Graphics.FromImage(bitmap);
                SolidBrush myBrush = new SolidBrush(ColorPanel.BackColor);
                g.FillRectangle(myBrush, new Rectangle(0, 0, (int)(ObjectSizeNumber.Value * XNumber.Value), (int)(ObjectSizeNumber.Value * YNumber.Value)));

                this.Cursor = CreateCursor(bitmap, 0, 0);
                bitmap.Dispose();
                isOnMap = true;
            }
        }

        private void MapPanel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = NormalCursor;
            isOnMap = false;
        }

        public bool isOnMap = false;

        private void MapPanel_Paint(object sender, PaintEventArgs e)
        {
            currentMap.DrawMap(e.Graphics, currentSelected);
        }

        private void MapPanel_MouseClick(object sender, MouseEventArgs e)
        {
            UpperPanel.Focus();
            if (ObjectButton.Enabled == false)
            {
                MapObject newObj = new MapObject();
                newObj.Location = e.Location;
                newObj.ObjectName = ObjectNameTextBox.Text;
                newObj.Size = (int)ObjectSizeNumber.Value;
                newObj.XLength = (int)XNumber.Value;
                newObj.YLength = (int)YNumber.Value;
                newObj.ObjectColor = ColorPanel.BackColor;
                currentMap.objects.Add(newObj);
            }
            MapPanel.Refresh();
        }

        private void PointerButton_Click(object sender, EventArgs e)
        {
            PointerButton.Enabled = false;
            ObjectButton.Enabled = true;
        }

        private void ObjectButton_Click(object sender, EventArgs e)
        {
            PointerButton.Enabled = true;
            ObjectButton.Enabled = false;
            currentSelected.Clear(); ;
        }

        private void ColorBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColorPanel.BackColor = (Color)ColorBox.SelectedItem;
        }

        Cursor NormalCursor;
        Map currentMap;
        List<int> currentSelected;

        private void MapPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (currentSelected.Count > 0)
                {
                    List<MapObject> newList = new List<MapObject>();
                    for (int i = 0; i < currentMap.objects.Count; i++)
                    {
                        if (!currentSelected.Contains(i))
                        {
                            newList.Add(currentMap.objects[i]);
                        }
                    }
                    currentMap.objects.Clear();
                    currentMap.objects.AddRange(newList);

                    currentSelected.Clear();
                    MapPanel.Refresh();
                }
            }
        }

        private void MapPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (!PointerButton.Enabled)
            {
                mouseDown = true;
                mouseDownLocation = e.Location;
            }
        }

        private void MapPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (!PointerButton.Enabled)
            {
                mouseDown = false;
                mouseUpLocation = e.Location;
                currentSelected.Clear();
                int left = Math.Min(mouseDownLocation.X, mouseUpLocation.X);
                int right = Math.Max(mouseDownLocation.X, mouseUpLocation.X);
                int top = Math.Min(mouseDownLocation.Y, mouseUpLocation.Y);
                int bottom = Math.Max(mouseDownLocation.Y, mouseUpLocation.Y);


                for (int i = 0; i < currentMap.objects.Count; i++)
                {
                    if (!(left > (currentMap.objects[i].Location.X + currentMap.objects[i].XLength * currentMap.objects[i].Size) || right < currentMap.objects[i].Location.X ||
                        top > (currentMap.objects[i].Location.Y + currentMap.objects[i].YLength * currentMap.objects[i].Size) || bottom < currentMap.objects[i].Location.Y))
                    {
                        currentSelected.Add(i);
                    }
                }
                MapPanel.Refresh();
            }
        }

        private void MapPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown && !PointerButton.Enabled)
            {
                mouseUpLocation = e.Location;
                UpperPanel.Refresh();
            }
        }

        bool mouseDown;
        Point mouseDownLocation;
        Point mouseUpLocation;

        private void UpperPanel_Paint(object sender, PaintEventArgs e)
        {
            if (mouseDown)
            {
                int left = Math.Min(mouseDownLocation.X, mouseUpLocation.X);
                int right = Math.Max(mouseDownLocation.X, mouseUpLocation.X);
                int top = Math.Min(mouseDownLocation.Y, mouseUpLocation.Y);
                int bottom = Math.Max(mouseDownLocation.Y, mouseUpLocation.Y);

                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(left, top, right - left, bottom - top));
            }
        }

        private void newMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentMap = new Map();
            currentSelected.Clear();
            UpperPanel.Refresh();
            MapPanel.Refresh();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentMap.LoadMap(openFileDialog1.FileName);
            }
            currentSelected.Clear();
            MapPanel.Refresh();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentMap.SaveMap(saveFileDialog1.FileName + ".txt");
            }
            MapName.Text = Path.GetFileName(saveFileDialog1.FileName);
        }
    }
}

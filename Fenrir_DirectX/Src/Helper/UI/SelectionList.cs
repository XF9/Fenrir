using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Fenrir.Src.Helper.UI.Alignment;

namespace Fenrir.Src.Helper.UI
{
    class ItemSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// the new selection index
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// the new selected string
        /// </summary>
        public String Selected { get; set; }
    }

    /// <summary>
    /// a list to make selections
    /// </summary>
    class SelectionList : Element
    {

        /// <summary>
        /// data to choose from
        /// </summary>
        private List<String> data;

        /// <summary>
        /// the data to choose from
        /// </summary>
        public List<String> Data
        {
            get { return data; }
            set { 
                data = value;
                this.buildList();
            }
        }

        private Vector2 position;
        public Vector2 Position
        {
            get { return this.position; }
            set { this.ResetPosition(value); }
        }

        /// <summary>
        /// list of labels for the data
        /// </summary>
        private List<TextButton> entries;

        /// <summary>
        /// label for description
        /// </summary>
        private Label label;

        /// <summary>
        /// the currently selected entry, button to open the selection list
        /// </summary>
        private TextButton currentSelection;

        public String CurrentValue
        {
            get { return this.currentSelection.Label.Text; }
        }

        /// <summary>
        /// the font to be used
        /// </summary>
        private String font;

        private int index;
        /// <summary>
        /// the index of the displayed entry
        /// </summary>
        public int Index
        {
            get { return index; }
            set { this.resetIndex(value);  }
        }

        private bool open;
        /// <summary>
        /// wether the selection list is open or not
        /// </summary>
        public bool Open
        {
            get { return open; }
            set { open = value; }
        }

        public event EventHandler<ItemSelectedEventArgs> onSelected;

        /// <summary>
        /// Creates the SelectionList
        /// </summary>
        /// <param name="yPos"></param>
        /// <param name="font"></param>
        /// <param name="label"></param>
        /// <param name="data"></param>
        /// <param name="dataIndex"></param>
        public SelectionList(String text, String font, List<String> data, int defaultIndex, Horizontal horizontalAlignment, Vertical verticalAlignment, Vector2 position = new Vector2())
        {
            this.data = data;
            this.entries = new List<TextButton>();
            this.font = font;
            this.index = defaultIndex;
            this.position = position;

            this.buildList();

            this.label = new Label(text, font, Horizontal.Left, Vertical.Top);
            this.ResetPosition(position);
        }

        /// <summary>
        /// build all entries in this selection
        /// </summary>
        private void buildList()
        {
            this.entries.Clear();

            for (int i = 0; i < this.data.Count; i++)
            {
                TextButton entry = new TextButton(data[i], this.font, Horizontal.Left, Vertical.Top);
                entry.onClick += HandleSelection;
                entry.onClick += ToggleMenu;
                this.entries.Add(entry);
            }

            this.resetIndex(this.index);
        }

        /// <summary>
        /// Menu handler for clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleMenu(object sender, EventArgs e)
        {
            this.ToggleMenu(null);
        }

        /// <summary>
        /// Toggle the menu
        /// </summary>
        public void ToogleMenu()
        {
            this.ToggleMenu(null);
        }

        /// <summary>
        /// Toggle the menu
        /// </summary>
        /// <param name="open">wether to be open or not</param>
        public void ToggleMenu(bool? open)
        {
            this.open = open ?? !this.open;
        }

        /// <summary>
        /// Sets a new index
        /// </summary>
        /// <param name="newIndex"></param>
        private void resetIndex(int newIndex)
        {
            if (this.index < 0)
                this.index = 0;
            if (this.index >= this.entries.Count)
                this.index = this.entries.Count - 1;
            else
                this.index = newIndex;

            this.currentSelection = this.entries[this.index].Copy();
            this.currentSelection.Position = new Vector2(this.position.X + 50, this.Position.Y);
            this.currentSelection.onClick += ToggleMenu;
        }

        /// <summary>
        /// Recalculate the position of this thing
        /// </summary>
        /// <param name="newPosition"></param>
        private void ResetPosition(Vector2 newPosition)
        {
            this.position = newPosition;

            this.label.Position = new Vector2(this.position.X - 200, this.position.Y);
            this.currentSelection.Position = new Vector2(this.position.X + 50, this.Position.Y);

            for (int i = 0; i < this.entries.Count; i++)
                this.entries[i].Position = new Vector2(this.position.X + 250, this.Position.Y + i * 30);
        }

        /// <summary>
        /// Updateds the thing
        /// </summary>
        public void Update()
        {
            this.currentSelection.Update();

            if(this.open)
                foreach (TextButton selection in this.entries)
                    selection.Update();
        }

        /// <summary>
        /// Draw it
        /// </summary>
        public void Draw()
        {
            this.label.Draw();
            this.currentSelection.Draw();

            if(this.open)
                foreach (TextButton selection in this.entries)
                    selection.Draw();
        }

        /// <summary>
        /// eventhandler for selecting a new item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleSelection(object sender, EventArgs e)
        {
            this.resetIndex(this.entries.IndexOf((TextButton)sender));
            ItemSelectedEventArgs ev = new ItemSelectedEventArgs();
            ev.Id = this.index;
            ev.Selected = this.currentSelection.Label.Text;
            this.EmitOnSelected(ev);
        }

        /// <summary>
        /// Value changed handler
        /// </summary>
        /// <param name="e"></param>
        protected virtual void EmitOnSelected(ItemSelectedEventArgs e)
        {
            EventHandler<ItemSelectedEventArgs> handler = onSelected;
            if (handler != null)
                handler(this, e);
        }
    }
}

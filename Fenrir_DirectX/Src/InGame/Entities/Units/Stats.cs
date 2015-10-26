using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fenrir.Src.InGame.Entities.Units
{
    /// <summary>
    /// Unit Properties / Statistics
    /// </summary>
    class Stats
    {
        private String name = "";
        /// <summary>
        /// the name of the unit
        /// </summary>
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        private int age = 0;
        /// <summary>
        /// the age of the unit
        /// </summary>
        public int Age
        {
            get 
            { 
                //return age; 
                return FenrirGame.Instance.Properties.CurrentGameTime.TotalGameTime.Minutes;
            }
            set { age = value; }
        }

        private int hunger = 0;
        /// <summary>
        /// the fed state of the unit
        /// </summary>
        public int Hunger
        {
            get { return hunger; }
            set { hunger = value; }
        }

        private float miningUpdateCounter = 0;
        private int mining;
        /// <summary>
        /// the mining skills
        /// </summary>
        public int Mining
        {
            get { return mining; }
            set {
                miningUpdateCounter++;
                if (miningUpdateCounter > 1000)
                {
                    mining = value;
                    miningUpdateCounter = 0;
                }
            }
        }

        private int construction = 0;
        /// <summary>
        /// the coinstruction skills
        /// </summary>
        public int Construction
        {
            get { return construction; }
            set { construction = value; }
        }

        private int fighting = 0;
        /// <summary>
        /// the fighting skills
        /// </summary>
        public int Fighting
        {
          get { return fighting; }
          set { fighting = value; }
        }

        private int wisdom = 0;
        /// <summary>
        /// the wisdom skills
        /// </summary>
        public int Wisdom
        {
            get { return wisdom; }
            set { wisdom = value; }
        }

        private int cooking = 0;
        /// <summary>
        /// the cooking skills
        /// </summary>
        public int Cooking
        {
            get { return cooking; }
            set { cooking = value; }
        }

        private StatsPanel panel;

        internal StatsPanel Panel
        {
            get { return panel; }
            set { panel = value; }
        }

        public Stats()
        {
            this.panel = new StatsPanel(this);

        }
    }
}

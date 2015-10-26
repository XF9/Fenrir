using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fenrir.Src.InGame.Entities.Units
{
    /// <summary>
    /// A task to perform by a unit
    /// </summary>
    interface ITask
    {
        /// <summary>
        /// prepare the task
        /// </summary>
        void Prepare(Entities.Unit executingUnit);

        /// <summary>
        /// execute the task
        /// </summary>
        void Execute();

        /// <summary>
        /// cancel the task
        /// </summary>
        void Cancel();

        /// <summary>
        /// pause the task
        /// </summary>
        void Pause();

        /// <summary>
        /// resume the paused task
        /// </summary>
        void Resume();

        /// <summary>
        /// check if paused
        /// </summary>
        /// <returns>if paused or not</returns>
        Boolean IsPaused();
    }
}

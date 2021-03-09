using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.UserInterface
{
    /// <summary>
    /// Stores generic states in structure that implements undo-redo functionality.
    /// </summary>
    /// <typeparam name="T">Type of UndoRedoStack state</typeparam>
    class UndoRedoStack<T>
    {
        /// <summary>
        /// Stack of states that can be undone.
        /// </summary>
        protected Stack<T> undoStack;

        /// <summary>
        /// Stack of states that can be redone.
        /// </summary>
        protected Stack<T> redoStack;

        /// <summary>
        /// Constructor for UndoRedoStack.
        /// </summary>
        public UndoRedoStack()
        {
            undoStack = new Stack<T>();
            redoStack = new Stack<T>();
        }

        /// <summary>
        /// Adds a state which can be undone.
        /// </summary>
        /// <param name="state">State to add.</param>
        public void AddState(T state)
        {
            // Create a copy of the state through serialization and deserialization
            JsonSerializerSettings settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, PreserveReferencesHandling = PreserveReferencesHandling.All };
            string data = JsonConvert.SerializeObject(state, Formatting.Indented, settings);
            T copiedState = JsonConvert.DeserializeObject<T>(data, settings);

            // Push state to undo stack and clear redo stack
            undoStack.Push(copiedState);
            redoStack.Clear();
        }

        /// <summary>
        /// Gets the previously added state if able to.
        /// </summary>
        /// <returns>Undone state.</returns>
        public T Undo()
        {
            if (CanUndo())
            {
                // Transfer the state at top of undo stack to top of redo stack and return the previously added state (peek top of undo stack)
                T undoneState = undoStack.Pop();
                redoStack.Push(undoneState);
                return undoStack.Peek();
            }
            else
            {
                // If stack has only one state, then only peek the current state from the undo stack, rather than popping.
                return undoStack.Peek();
            }
        }

        /// <summary>
        /// Gets the previously undone state if able to.
        /// </summary>
        /// <returns>Redone state.</returns>
        public T Redo()
        {
            if (CanRedo())
            {
                // Transfer the state at top of redo stack to top of undo stack and return the redone state
                T redoneState = redoStack.Pop();
                undoStack.Push(redoneState);
                return redoneState;
            }
            else
            {
                // If stack has only one state, then only peek the current state from the undo stack, rather than popping from the redo stack.
                return undoStack.Peek();
            }
        }

        /// <summary>
        /// Gets if a previous state can be recovered.
        /// </summary>
        /// <returns></returns>
        public bool CanUndo()
        {
            return undoStack.Count > 1;
        }

        /// <summary>
        /// Gets if a previously undone state can be recovered.
        /// </summary>
        /// <returns></returns>
        public bool CanRedo()
        {
            return redoStack.Count > 0;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.UserInterface
{
    class UndoRedoStack<T>
    {
        protected Stack<T> undoStack;

        protected Stack<T> redoStack;

        public UndoRedoStack()
        {
            undoStack = new Stack<T>();
            redoStack = new Stack<T>();
        }

        public void AddState(T state)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, PreserveReferencesHandling = PreserveReferencesHandling.All };
            string data = JsonConvert.SerializeObject(state, Formatting.Indented, settings);
            T copiedState = JsonConvert.DeserializeObject<T>(data, settings);

            undoStack.Push(copiedState);
            redoStack.Clear();
        }

        public T Undo()
        {
            if (CanUndo())
            {
                T undoneState = undoStack.Pop();
                redoStack.Push(undoneState);
                return undoStack.Peek();
            }
            else
            {
                return undoStack.Peek();
            }
        }

        public T Redo()
        {
            if (CanRedo())
            {
                T redoneState = redoStack.Pop();
                undoStack.Push(redoneState);
                return redoneState;
            }
            else
            {
                return undoStack.Peek();
            }
        }

        public bool CanUndo()
        {
            return undoStack.Count > 1;
        }

        public bool CanRedo()
        {
            return redoStack.Count > 0;
        }
    }
}

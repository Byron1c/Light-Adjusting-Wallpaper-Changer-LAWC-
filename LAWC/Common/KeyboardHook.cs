using System;
using System.Windows.Forms;
using System.Globalization;


namespace LAWC.Common
{
    public sealed class KeyboardHook : IDisposable
    {

        private class Window : NativeWindow, IDisposable
        {
            // not sure where this came from

            private const int WM_HOTKEY = 0x0312;

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
            internal Window()
            {
                // create the handle for the window.
                CreateHandle(new CreateParams());//this.CreateHandle(new CreateParams());
            }


            /// <summary>
            /// Overridden to get the notifications.
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                // check if we got a hot key pressed.
                if (m.Msg == WM_HOTKEY)
                {
                    // get the keys.
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

                    // invoke the event to notify the parent.
                    KeyPressed?.Invoke(this, new KeyPressedEventArgs(modifier, key));
                }
            }


            public event EventHandler<KeyPressedEventArgs> KeyPressed;

            #region IDisposable Members

            public void Dispose()
            {
                this.DestroyHandle();
            }

            #endregion
        }


        private readonly Window _window = new Window();
        private int _currentId;

        public KeyboardHook()
        {
            // register the event of the inner native window.
            _window.KeyPressed += delegate (object sender, KeyPressedEventArgs args)
            {
                KeyPressed?.Invoke(this, args);
            };
        }


        public static ModifierKeys KeyToModifierKey(Keys vKey)
        {
            if (vKey.ToString().Contains("Shift")) return ModifierKeys.Shift;
            if (vKey.ToString().Contains("Alt")) return ModifierKeys.Alt;
            if (vKey.ToString().Contains("Control")) return ModifierKeys.Control;
            if (vKey.ToString().Contains("Win")) return ModifierKeys.Win;

            return ModifierKeys.None;
        }


        /// <summary>
        /// Registers a hot key in the system.
        /// </summary>
        /// <param name="modifier">The modifiers that are associated with the hot key.</param>
        /// <param name="key">The key itself that is associated with the hot key.</param>
        public int RegisterHotKey(ModifierKeys modifier, Keys key)
        {
            // increment the counter.
            _currentId += 1;

            // register the hot key.
            if (modifier != ModifierKeys.None)
            {
                if (!NativeMethods.RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
                    throw new InvalidOperationException("Couldn’t register the hot key.");
            }
            else
            {
                throw new InvalidOperationException("Couldn’t register the hot key. You Need a modifier.");
            }

            return _currentId;
        }


        // incase user wants to use a fixed id each time
        public int RegisterHotKey(int vID, ModifierKeys modifier, Keys key)
        {

            // register the hot key.
            if (modifier != ModifierKeys.None)
            {
                if (!NativeMethods.RegisterHotKey(_window.Handle, vID, (uint)modifier, (uint)key))
                    throw new InvalidOperationException("Couldn’t register the hot key.");
            }
            else
            {
                throw new InvalidOperationException("Couldn’t register the hot key. You Need a modifier.");
            }

            return vID;
        }


        public void UnRegisterHotKey(int vID)
        {
            //if (vID <= _currentId)
            //{
            NativeMethods.UnregisterHotKey(_window.Handle, vID);
            //_currentId--;
            //}

        }


        /// <summary>
        /// A hot key has been pressed.
        /// </summary>
        public event EventHandler<KeyPressedEventArgs> KeyPressed;


        #region IDisposable Members

        public void Dispose()
        {
            // unregister all the registered hot keys.
            for (int i = _currentId; i > 0; i--)
            {
                NativeMethods.UnregisterHotKey(_window.Handle, i);
            }

            // dispose the inner native window.
            _window.Dispose();
        }

        #endregion
    }


    /// <summary>
    /// Event Args for the event that is fired after the hot key has been pressed.
    /// </summary>
    public class KeyPressedEventArgs : EventArgs
    {
        private readonly ModifierKeys _modifier;
        private readonly Keys _key;

        internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
        {
            _modifier = modifier;
            _key = key;
        }

        public ModifierKeys Modifier
        {
            get { return _modifier; }
        }

        public Keys Key
        {
            get { return _key; }
        }
    }

    /// <summary>
    /// The enumeration of possible modifiers.
    /// </summary>
    [Flags]
    public enum ModifierKeys : int//uint
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }




}

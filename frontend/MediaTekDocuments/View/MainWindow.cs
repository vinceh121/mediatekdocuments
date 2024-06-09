using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Gtk;
using MediaTekDocuments.Dal;
using MediaTekDocuments.Model;
using MediaTekDocuments.View.Tabs;
using UI = Gtk.Builder.ObjectAttribute;

namespace MediaTekDocuments.View
{
    public class MainWindow : ApplicationWindow
    {
        private readonly Program _program;

        [UI]
        private Notebook _notebook;

        private readonly TabBooks _tabBooks;

        public MainWindow(Program program, bool readOnly) : this(program, readOnly, new Builder("MainWindow.glade")) { }

        private MainWindow(Program program, bool readOnly, Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            this._program = program;
            this.Application = program.GetApplication();
            builder.Autoconnect(this);

            this._tabBooks = new TabBooks(program, readOnly);
            this._notebook.InsertPage(this._tabBooks, new Label("test"), 0);

            DeleteEvent += Window_DeleteEvent;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
    }
}

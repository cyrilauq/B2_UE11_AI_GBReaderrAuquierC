﻿using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using GBReaderAuquierC.Domains.Events;
using GBReaderAuquierC.Presentation;

namespace GBReaderAuquierC.Avalonia;

public partial class ExtendedDescriptionBookView : UserControl
{
    public ExtendedDescriptionBookView()
    {
        InitializeComponent();
        
    }

    public void SetInfos(BookExtendedItem item)
    {
        Title.Text = item.Title;
        Resume.Text = item.Resume;
        ISBN.Text = item.Isbn;
        Author.Text = item.Author;
        if (item.ImgPath != null && item.ImgPath.Trim().Length > 0)
        {
            Image.Source = new Bitmap(item.ImgPath);
        }
    }

    private void On_Show(object? sender, RoutedEventArgs e)
    {
        Show?.Invoke(this, new DescriptionEventArgs(ISBN.Text));
    }

    public event DescriptionEventHandler Show;
}
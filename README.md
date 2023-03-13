# QrCodeGenerator
Cross plafrom C# Qr code image generator

# Simple wrapper around:
https://github.com/manuelbl/QrCodeGenerator

## Advantages
* Select Driver even on runtime
* Robustness in case on some platforms some drawing libraries having problems
* Fluent API initialization (surely improvable)

## Example Syntax

```csharp

var qr = new QrDrawerFactory().GetDriver(DrawDriverType.SkiaSharp)
                .SetQr(msg)
                .SetImageFormat(CommonImageFormat.Png)
                .GetBytes();
                
var qr = new QrDrawerFactory().GetDriver(DrawDriverType.ImageSharp)
                .SetQr(msg)
                .SetImageFormat(CommonImageFormat.Png)
                .Save("./Test.png");

```

### Interface:

```csharp

public abstract GenericDrawer SetQr(QrCode QrCode);
public GenericDrawer SetQr(string QrText);

public abstract GenericDrawer SetScale(int Scale);
public abstract GenericDrawer SetBorder(int Border);
public abstract GenericDrawer SetForegroundColor(string Color);
public abstract GenericDrawer SetBackgroundColor(string Color);
public abstract GenericDrawer SetImageFormat(CommonImageFormat ImageFormat);

public abstract byte[] GetBytes();
public abstract bool Save(string Path);

```

### Drivers Support: 
* SkiaSharp / ImageSharp / (open to expansions)

# RhosynWatchies
A simple little Gorilla Tag watch library that you can easily slap into your project.

## Setup
- Place the "RhosynWatchies.cs" file into your project.
- (Optional) Add your namespace

## Usage
### Init
Simply put this wherever your mod is initiated.
```cs
RhosynWatchies.GetInstance(); 
```

## Varibles
- `autoShowWatch`: If the watch should show when created or not

## Functions

- `ShowWatch()`: Shows the watch.
- `HideWatch()`: Hides the watch.
- `SetText(string text)`: Sets the text of the watch.
- `SetIcon(Texture2D texture)`: Sets the icon of the watch.
- `SetColour(Watch.ColourType type, Colour colour)`: Sets the colour of the watch text or icon.
- `SetFontSize(int size)`: Sets the font size of the watch text.

---

### Example Usage


```csharp
var watch = RhosynWatchies.GetInstance();
watch.SetText("Hello, Monkeh!");
watch.SetFontSize(8);
watch.SetColour(RhosynWatchies.Watch.ColourType.Text, Color.green);
watch.SetIcon(myTexture2D);
watch.ShowWatch();
```

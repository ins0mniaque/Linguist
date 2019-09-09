[![Linguist](https://raw.githubusercontent.com/ins0mniaque/Linguist/master/content/images/logo.png)](https://github.com/ins0mniaque/Linguist)
[![CoreBadge]][Core]
[![License](https://img.shields.io/github/license/ins0mniaque/Linguist.svg)](LICENSE)
# Linguist
An advanced strongly typed resource localizer for all .NET platforms with formatting and pluralization support

<br>

## Getting started
* Coming soon; for now the demos and test projects are the best source of information. For Xamarin.Forms, since both the WPF and Xamarin.Forms markup extensions have the exact same features, also check out the WPF demo. 

## Features
* Supports integer, decimal and range plural rules from Unicode CLDR with a simple API
* Small with no dependencies supporting .NET 3.5, .NET 4.6 and .NET Standard 2.0
* Support for .NET Standard 2.0 binary serialization and System.Drawing.Common
* WPF (with XAML Designer support) and Xamarin.Forms markup extension with binding and automatic key generation
* Advanced resource class generator adding format methods with automatic support for pluralization
* Build task that adds a "Localization" build action that both generates the resource class and embeds resources allowing building on .NET Core without Visual Studio
* Support for reading .resx, .resw files directly without embedding them with more formats coming soon

## Roadmap to release
* Complete and publish build task, add satellite assembly tests
* Complete XML documentation
* Add "Getting Started" and more documentation
* Complete demos

<br>

## NuGet Packages
Install the following package to start using Linguist in your own app.

| Platform          | Package                          | NuGet                | Demo             |
| ----------------- | -------------------------------- | -------------------- | ---------------- |
| .NET Standard     | [Linguist][CoreLink]             | [![CoreBadge]][Core] | [Demo][CoreDemo] |
| .NET 4.6          |                                  |                      |                  |
| .NET 3.5          |                                  |                      |                  |
| WPF               | [Linguist.WPF][WPFLink]          | [![WPFBadge]][WPF]   | [Demo][WPFDemo]  |
| Xamarin.Forms     | [Linguist.Xamarin.Forms][XFLink] | [![XFBadge]][XF]     | [Demo][XFDemo]   |

[Core]: https://www.nuget.org/packages/Linguist/
[CoreBadge]: https://img.shields.io/nuget/v/Linguist.svg
[CoreLink]: https://github.com/ins0mniaque/Linguist/tree/master/src/Linguist
[CoreDemo]: https://github.com/ins0mniaque/Linguist/tree/master/samples/Linguist.Demo

[WPF]: https://www.nuget.org/packages/Linguist.WPF/
[WPFBadge]: https://img.shields.io/nuget/v/Linguist.WPF.svg
[WPFLink]: https://github.com/ins0mniaque/Linguist/tree/master/src/Linguist.WPF
[WPFDemo]: https://github.com/ins0mniaque/Linguist/tree/master/samples/Linguist.WPF.Demo

[XF]: https://www.nuget.org/packages/Linguist.Xamarin.Forms/
[XFBadge]: https://img.shields.io/nuget/v/Linguist.Xamarin.Forms.svg
[XFLink]: https://github.com/ins0mniaque/Linguist/tree/master/src/Linguist.Xamarin.Forms
[XFDemo]: https://github.com/ins0mniaque/Linguist/tree/master/samples/Linguist.Xamarin.Forms.Demo

## Tools
The following tools are available for development and customization.

| Platform          | Package / Extension                 | NuGet / Gallery                      |
| ----------------- | ----------------------------------- | ------------------------------------ |
| .NET Standard     | [Linguist.Generator][GeneratorLink] | [![GeneratorBadge]][Generator]       |
| .NET 4.6          |                                     |                                      |
| .NET 3.5          |                                     |                                      |
| Visual Studio     | [Linguist][VisualStudioLink]        | [![VisualStudioBadge]][VisualStudio] |

[Generator]: https://www.nuget.org/packages/Linguist.Generator/
[GeneratorBadge]: https://img.shields.io/nuget/v/Linguist.Generator.svg
[GeneratorLink]: https://github.com/ins0mniaque/Linguist/tree/master/src/Linguist.Generator

[VisualStudio]: https://marketplace.visualstudio.com/items?itemName=ins0mniaque.linguist
[VisualStudioBadge]: https://img.shields.io/visual-studio-marketplace/d/ins0mniaque.linguist.svg
[VisualStudioLink]: https://github.com/ins0mniaque/Linguist/tree/master/src/Linguist.VisualStudio

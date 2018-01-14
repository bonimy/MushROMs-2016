Turn on word wrap

Helper.ColorSpaces provides structures for viewing colors in different representations. Sometimes it is unsavory to perform operations on an RGB-style color, say if we wanted to shift a color's hue, lighten a color, etc.

It is also handy to provide blending options for two colors. Every photoshop color blend is implemented in ColorRgb. However, the conversions aren't always exact matches to what they are in photoshop. Refer to the remarks of each blend method in the ColorRgb class for more info as well as where the algorithm was obtained.

Every structure represents a floating-point color space that uniquely defines a color. Implicit type conversions are provided from every color space to the other. An explicit type conversion for each color space to a System.Drawing.Color is also provided, as well as an implicit conversion of the reverse.

Color equality is satisfied if two color spaces describe the same "color". That is, if a CMY color describes pure red, an equality comparison to an RGB color that describes pure red will return true. Behind the scenes, a color comparison converts both colors to RGB colors first, then compares their values. Equality of each channel is satisfied if they are within a predefined tolerance (specified in its class). Equality with System.Drawing.Color is tested by converting the RGB color to a System.Drawing.Color. Note that named colors do no return as equal to their unnamed colors even if they have the same ARGB values, but this is a fact of the .NET framework itself and cannot be circumvented. Hash codes between different color structures will also be the same if they describe the same color.

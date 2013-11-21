# KindleBookHelper

## Overview
Converts raw text file to MOBI html format that can be consumed by KindleGen/Previewer.
It provides fairly basic formatting/functionality and only includes code for poetry at the moment.
It will generate navigation, including a linkable table of contents.
Some additional text can be included via the json parameter.

## Output
All files utilize an "URL-friendly" version of the book title (lowercase with special characters and spaces encoded or removed)
- book-title.opf. Xml document that serves as the book's manifest. This is the file fed into KindleGen or Previewer.
- book-title.ncx. Xml document that lays out the book's navigation.
- book-title.html. Html document that has all content.
- poetry.css. Stylesheet used by book.

## Assumptions
- JSON file is provided (location set via application config file)
- The first line is the name of the poem
- Each stanza is separated by a line break
- Each poem is separated by "[END]" text
- A unique identifier Id field will be generated on first run; be sure to clear it if re-using json for additional books.
- CSS is initially pulled from the application DLL. It will not override it, so you can edit the file freely.

## Examples

### JSON config file

```json
{
  "RawFilePath": "G:\\Documents\\Books\\Book Title\\raw.txt",
  "Title": "Book Title",
  "CoverFileName": "cover.jpg",
  "Publisher": "Author Name or Company",
  "Creator": "Author Name",
  "Author": "Author Name",
  "AuthorAlphabetical": "Last, First",
  "AuthorDescription": "Author description",
  "AuthorWebsite": "Author web site (no http://)",
  "Forward": "Forward text"
}
```

### Raw text file

```
A Title

First Line
Second Line

Another Stanze
Last Line

[END]

Another Title

Another line
Yet Another line

[END]
```
# KindleBookHelper

## Overview
Converts raw text file to MOBI html format that can be consumed by KindleGen/Previewer.
It provides fairly basic formatting/functionality and only includes code for poetry at the moment.
It will generate navigation, including a linkable table of contents to each poem.
Some additional text can be included via the book metadata file.

## Output
All files utilize an "URL-friendly" version of the book title (lowercase with special characters and spaces encoded or removed)
- book-title.opf. Xml document that serves as the book's manifest. This is the file fed into KindleGen or Previewer.
- book-title.ncx. Xml document that lays out the book's navigation.
- book-title.html. Html document that has all content.
- poetry.css. Stylesheet used by book.

## Usage
- -f JSON file with book metadata (required)
- -e Text marking the end of each poem (optional; defaults to "[END]")
- -p If provided, pauses the application after running (useful if running within an IDE)

## Notes
- Program assumes that the first line is the name of the poem and that each stanza is separated by a line break
- A unique identifier Id field will be generated on first run; be sure to clear it if re-using json for additional books.
- CSS is initially pulled from the application DLL. It will not override it, so you can edit the file freely.

## Examples

### Book Metadata

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

Another Stanza
Last Line

[END]

Another Title

Another line
Yet Another line
The line to end all lines

[END]
```
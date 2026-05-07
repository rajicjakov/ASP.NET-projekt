# Semantic Database Model

## Entities / Tables

### User
- `Id` [Key]
- `Username`
- `Email`
- `DateJoined`
- `Role`
- `Tabs` (virtual `ICollection<Tab>`) — 1-N relationship to `Tab` via `Tab.CreatorId`

### Tab
- `Id` [Key]
- `Title`
- `Artist`
- `CreatorId` [ForeignKey("Creator")]
- `Creator` (virtual `User`)
- `DateCreated`
- `StringTuning`
- `BPM`
- `Difficulty`
- `Measures` (virtual `ICollection<TabMeasure>`) — 1-N relationship to `TabMeasure` via `TabMeasure.TabId`

### TabMeasure
- `Id` [Key]
- `TabId` [ForeignKey("Tab")]
- `Tab` (virtual `Tab`)
- `MeasureNumber`
- `TimeSignatureTop`
- `TimeSignatureBottom`
- `Columns` (virtual `ICollection<TabColumn>`) — 1-N relationship to `TabColumn` via `TabColumn.TabMeasureId`

### TabColumn
- `Id` [Key]
- `TabMeasureId` [ForeignKey("TabMeasure")]
- `TabMeasure` (virtual `TabMeasure`)
- `ColumnNumber`
- `ColumnDuration` (required `Duration`)
- `Notes` (virtual `ICollection<TabNote>`) — 1-N relationship to `TabNote` via `TabNote.TabColumnId`

### TabNote
- `Id` [Key]
- `TabColumnId` [ForeignKey("TabColumn")]
- `TabColumn` (virtual `TabColumn`)
- `StringNumber`
- `Fret`
- `PalmMuted`
- `HammerOn`
- `PullOff`
- `Bend`

### Duration
- `Id` [Key]
- `Base`
- `IsDotted`
- `TabColumnId` [ForeignKey("TabColumn")]
- `TabColumn` (virtual `TabColumn`)

## Relationships
- `User` 1 — N `Tab` via `Tab.CreatorId`
- `Tab` 1 — N `TabMeasure` via `TabMeasure.TabId`
- `TabMeasure` 1 — N `TabColumn` via `TabColumn.TabMeasureId`
- `TabColumn` 1 — N `TabNote` via `TabNote.TabColumnId`
- `TabColumn` 1 — 1 `Duration` via `Duration.TabColumnId`

## Notes
- There are no N-N entity relationships defined in the current model.
- `BrowseViewModel` is present in the project but is a view model, not a database entity.

# Collection Manager

An extensive tool for creating, editing, and exporting of osu! collections.

## Download

You can download the latest release of Collection Manager here: [Releases](https://github.com/Piotrekol/CollectionManager/releases/latest). Files to download are in the `Assets` dropdown at the bottom of the release.

For Windows, download `CollectionManagerSetup.exe`.

For Linux, through wine, you likely need to use `CollectionManagerSetup_linux.exe`. The same may be true for MacOS.

## Setup

On the first start of Collection Manager, it will attempt to automatically detect your osu! folder. If it detects incorrectly or cannot automatically detect your osu! folder, you will have to select it manually.

To reset this setting, go to Collection Manager's top menu and select `Settings > Reset` then restart Collection Manager.

If your osu! folder is not configured in Collection Manager, .db collections will only show hashes instead of identifying maps using your osu! songs, you will not be able to open or save to your osu! collection directly and you will not be able to view the beatmap listing in Collection Manager. Most features will still function.

## Usage

### **Opening Collections**

Collections can be loaded into Collection Manager in three ways:

- Going to the top menu, selecting `File > Open` and either `Collection(.db/.osdb)` or `osu! collection`

- Dragging a collection file into the left panel.

- Using previously uploaded osu!Stats collections. See: [osu!Stats Collections](https://github.com/Piotrekol/CollectionManager#osustats-collections)

### **Saving Collections**

Collections can be saved by going to the top menu, selecting `File > Save` and any of the following options:

- `Collection(.db/.osdb)`: This will allow you to save the currently loaded collections in either .db format or .osdb format.

  .osdb format stores more information about the collection and is preferable for sharing collections.

- `osu! collection`: This will **overwrite** your existing osu! collections with the currently loaded Collection Manager collections.

  It is best to have a backup of your osu! collections before saving in this way.

- `Collections in separate files`: This will export each individual collection that is loaded as its own file instead of saving all collections in a single file. You will be prompted to select a folder to save them in and whether or not you want to save them in .osdb format.

In addition to these methods of saving collections, they can also be exported as a list in plaintext by going to the top menu and selecting `Listing` then either of the following options:

- `List all collections`: This will generate a list containing every collection loaded. Each collection is followed by a list of maps contained in that collection.

- `List missing maps`: This will generate a list containing every collection loaded that has missing maps. Each collection is followed by a list of missing maps contained in that collection.

Collection Manager **cannot** open or import these lists back into collections. They can only be exported from collections that are already loaded. Make sure to save your collections in .db or .osdb format in addition to this.

### **Collection File Formats**

Collection Manager can handle the following two types of collection formats:

- .db Collections: These are collections in the format that osu! uses. They store collection names and map hashes. Maps in these collections cannot be identified locally without an osu! songs folder and an osu!.db file. For more information, see: [Legacy database file structure](https://github.com/ppy/osu/wiki/Legacy-database-file-structure#collectiondb).

- .osdb Collections: These are collections in Collection Manager's format. They store collection names, map hashes, mapIDs, mapsetIDs, star ratings, and map states. It is recommended to use this format when sharing collections due to it not requiring any osu! files to identify maps.

  However, **the initial save of the .osdb file must have all the info desired.** Opening a .db collection or any collection with missing data and saving it as a .osdb collection will not repair any of the missing data unless the missing maps and data are also present in your osu!.db and songs folder.

### **Downloading Maps from Collections**

Collection Manager can automatically download maps for you if you load in a .osdb collection with maps you do not already have. **You cannot use a .db collection for downloading.** 

Excessive downloading of maps could lead to your osu! account being temporarily download banned by the osu! servers. Collection Manager has no control over this, but to help prevent download bans, Collection Manager limits downloads to 170 mapsets per hour and 3 mapsets per minute.

To start downloading missing maps from all loaded collections:

Go to the top menu, select `Online > Download all missing maps`, select a directory to download to, then enter in cookies. To get your cookies follow this [Tutorial](https://streamable.com/lhlr3d) for getting cookies on Chrome (it should be nearly the same on all browsers). Make sure you are logged in to your osu! account and when copying the cookie, get the entire string after `cookie:`. **Do not use `set-cookie:` or any other headers.**

**DO NOT** share these cookies with anyone else. They contain temporary information for your browser to **access your account** which is required for downloads.

Download progress can be monitored in the `Download list` window. If closed, this window can be restored by selecting `Online > Show map downloads` from the top menu. The `Progress` column can be expanded using the dividers at the top to check when new downloads will start. Throttled downloads will show this message when the column is not expanded: `Next download slot a...`. This does not mean downloads have failed. They will automatically start again.

### **Generating Collections**

Selecting `Online > Generate collection` will bring up the `Collection Generator`.

This can be used to create collections from the top plays of specific players.

Once collections are generated, upon closing the `Collection Generator` they will be loaded into the collection listing.

### **Creating and Editing Collections**

The following opinions are made available by right clicking in the left panel:

- Create: Prompts you to create a new collection. 

  Using a name that does not match any existing collections is required.

- Rename: Prompts you to rename a collection. 

  Using a name that does not match any existing collections is required.

- Delete: Deletes all selected collections.

- Duplicate: Duplicates a single collection with `_0` appended to the new collection's name.

- Merge Selected: Merges all selected collections into the selected collection closest to the top of the listing.

- Intersection: Intersects all selected collections by making a new collection with only maps that are present in all selected collections.

  The new collection is named using the selected collection closest to the top of the listing, with `_0` appended to its name.

- Inverse: Inverses all selected collections by making a new collection with only maps that are not present in any of the selected collections but are present in your osu! songs folder.

  The new collection is named using the selected collection closest to the top of the listing, with `_0` appended to its name.

### **Adding Maps to Collections**

Maps can be added to existing collections in two ways:

- Selecting a collection, then dragging maps from the middle panel into a different collection on the left panel.

- Selecting `Show beatmap listing` from the top menu, then dragging maps from the beatmap listing into specific collections on the left panel or dragging maps into the map listing for the currently selected collection on the middle panel.

  Collection Manager must have your osu folder path set up for this to work.

### **osu!Stats Collections**

Collection Manager can be integrated into osu!Stats for uploading local collections and loading in online collections.

First, you will need to log in using your osu!Stats api key. You can get your api key [here](https://osustats.ppy.sh/collections/apikey) while logged in to osu!Stats. Enter your api key into Collection Manager by going to `Osustats collections > Login...` on the top menu. If you need to change your api key later, you can do so by selecting `Osustats collections > Logged in as {username}` and inputting the api key.

After you've entered your api key, you can upload your own collections to osu!Stats and load your uploaded collections into Collection Manager.

If you have a collection with missing maps, osu!Stats may be able to repair the collection and find those missing maps for you. To do this, upload the collection to osu!Stats by going to `Osustats collections > Upload new collection` and selecting the collection to upload. osu!Stats will then process the collection and if there are any missing maps that can be identified, they will be found. Processing of some collections can take a long time. You may also need to restart Collection Manager and load the collection from `Osustats collections > Your collections` to get it to refresh.

## CLI Usage

The following command-line options are supported:

`-o` / `--Output`: Required. Output filename with or without a path. The filename extension will specify which format to save in: `.db` or `.osdb`. 

`-b` / `--BeatmapIds`: Comma or whitespace separated list of beatmap ids. This can also be a path to a file containing this list.

`-i` / `--Input`: Input .db/.osdb collection file.

`-l` / `--OsuLocation`: The location of your osu! directory or a directory containing a valid osu!.db. If not provided, Collection Manager will attempt to find it automatically.

`-s` / `--SkipOsuLocation`: Skip loading of osu! database.

`--help`: Display the help screen.

`--version`: Display version information.

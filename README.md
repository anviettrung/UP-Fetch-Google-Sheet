﻿# Fetch Google Sheet

> [Description](#description) 
| [Installation](#installation)
| [Usage](#usage)
| [FAQ](#frequently-asked-questions)

## Description

Fetch data from Google Sheet to Unity.

## Installation

#### Requirement

* Unity 2020.3 or later
* Install the package `@com.unity.editorcoroutines` (if not installed automatically)

#### Using Unity Package Manager
  
To use this in your Unity project import it from Unity Package Manager. You can [download it and import it from your hard drive](https://docs.unity3d.com/Manual/upm-ui-local.html), or [link to it from github directly](https://docs.unity3d.com/Manual/upm-ui-giturl.html).

> Git Url to install:
> https://github.com/anviettrung/UP-Fetch-Google-Sheet.git

## Usage

**Step 1**: Declare a `FetchConfig` variable, for example:

```cs
[Fetch("OnFetchSuccess")] 
public FetchConfig fetchConfig;
```

In the example above, `OnFetchSuccess` is the name of the callback that will be called upon successful fetching.

**Step 2**: Fill in all the fields in the config, for example:

<img width="368" alt="image" src="https://github.com/anviettrung/UP-Fetch-Google-Sheet/assets/40160468/46c0a5b2-4478-40a9-a7aa-0a5fda6ac48b">

- **Source**: URL of published sheet.
- **Gid**: ID of sheet tab.
- **Format**: Sheet format which is comma-separated values (CSV) or tab-separated values (TSV).
- **Has Header**: Use the first row of the selected range to define field name. 
- **Range**: Selected range (use A1 notation).

For more information on how to fill in all the fields above, please refer to the section [FAQ](#frequently-asked-questions).

**Step 3**: Define callback `OnFetchSuccess`. For example, fill data into the list `units`:

```cs
public List<UnitData> units;

private void OnFetchSuccess(SheetTable table)
{
    FetchGoogleSheet.SheetTableToList(table, units);
}
```

Definition of type `UnitData`:

```cs
public struct UnitData : IGoogleSheetDataSetter
{
    public string name;
    public int health;
    public int damage;

    public void SetDataFromSheet(SheetRecord record)
    {
        name = record["name"];
        health = int.Parse(record["health"]);
        damage = int.Parse(record["damage"]);
    }
}
```

**Sample data** (can be found [here](https://docs.google.com/spreadsheets/d/1x0M9_qgQiVXtdWL3DXXnf4Pp2fkVALfHcHoqETKwCnY/edit?usp=sharing))

<img width="352" alt="image" src="https://github.com/anviettrung/UP-Fetch-Google-Sheet/assets/40160468/2fda61c3-a6ac-427f-9deb-be7eb0579703">

**Step 4**: Press button `Fetch` on the inspector. Wait a few secs then data will be filled into the list.

<img width="380" alt="image" src="https://github.com/anviettrung/UP-Fetch-Google-Sheet/assets/40160468/4b28183b-7f44-42db-a4ca-090313e52bde">

## Sample

Import the samples in the package to learn more ways to fetching different kinds of data.

To import, navigate to **Window -> Package Manager**, select the **Fetch Google Sheet** package, and click the `Import` button under the **Samples** section.

<img width="238" alt="image" src="https://github.com/anviettrung/UP-Fetch-Google-Sheet/assets/40160468/791bb4ae-24bb-4da2-b49d-fd94a79d3a70">

## Frequently Asked Questions

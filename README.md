# Fetch Google Sheet

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

**Step 1**: Declare a **FetchConfig** variable, for example:

```cs
[Fetch("OnFetchSuccess")] 
public FetchConfig fetchConfig;
```

In the example above, `OnFetchSuccess` is the name of the callback that will be called upon successful fetching.

**Step 2**: Fill in all the fields in the config, for example:

- **Source**: URL of published sheet.
- **Gid**: ID of sheet tab.
- **Format**: Sheet format which is comma-separated values (CSV) or tab-separated values (TSV).
- **Has Header**: Use the first row of the selected range to define field name. 
- **Range**: Selected range (use A1 notation).

For more information on how to fill in all the fields above, please refer to the section [FAQ](#frequently-asked-questions).

**Step 3**: Define callback `OnFetchSuccess`. For example, fill data into the list `units`:

```cs
```

Definition of type `UnitData`:
```cs
```

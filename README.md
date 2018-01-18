[![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.1154950.svg)][doi]

# Introduction

Pulls down all the SEC filings from [EDGAR][edgar] by filing type for a given time range.

# How to use

1. Open a command prompt
2. Change to the directory where you downloaded the files
3. Run `ScrapeSecEdgar.exe`

# Parameters

`ScrapeSecEdgar.exe` has 4 optional parameters

1. formtype
  * The SEC form type. Found in the master.idx 'Form Type' column.
  *  Defaults to 10-K
  * Alias: f
2. start
  * The date to start the pull. Found in the master.idx 'Date Filed' column.
  * Defaults to 1993/01/10.
  * Alias: s
3. end
  * The date to end the pull. Found in the master.idx 'Date Filed' column.
  * Defaults to today.
  * Alias: e
4. path
  * The path to save files to.
  * Defaults to %Desktop%/EDGAR
  * Alias: p

```{shell}
ScrapeSecEdgar.exe -s 2017/08/05
```

---------

[edgar]: https://www.sec.gov/edgar.shtml
[doi]: https://doi.org/10.5281/zenodo.1154950

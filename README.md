# toofz Leaderboards Core (Data)

[![Build status](https://ci.appveyor.com/api/projects/status/belqgh64mubwul1u/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer-leaderboards-entityframework/branch/master)
[![codecov](https://codecov.io/gh/leonard-thieu/toofz-leaderboards-core-data/branch/master/graph/badge.svg)](https://codecov.io/gh/leonard-thieu/toofz-leaderboards-core-data)
[![MyGet](https://img.shields.io/myget/toofz/v/toofz.NecroDancer.Leaderboards.Data.svg)](https://www.myget.org/feed/toofz/package/nuget/toofz.NecroDancer.Leaderboards.Data)

## Overview

**toofz Leaderboards Core (Data)** serves as the data access layer (DAL) for leaderboards, players, and replays. Retrieving data is done through an Entity Framework Code First model. 
Modifying data uses a combination of `SqlBulkCopy` and raw SQL for performant bulk inserting and upserting.

---

**toofz Leaderboards Core (Data)** is a component of **toofz**. 
Information about other projects that support **toofz** can be found in the [meta-repository](https://github.com/leonard-thieu/toofz-necrodancer).

### Dependents

* [toofz Leaderboards Service](https://github.com/leonard-thieu/leaderboards-service)
* [toofz Players Service](https://github.com/leonard-thieu/players-service)
* [toofz Replays Service](https://github.com/leonard-thieu/replays-service)
* [toofz API](https://github.com/leonard-thieu/api.toofz.com)

## Requirements

* .NET Framework 4.6.1
* MS SQL Server

## Building and testing

Visual Studio 2017 (version 15.3 or later) can be used to build and run tests. Integration tests require MS SQL Server.

## License

**toofz Leaderboards Core (Data)** is released under the [MIT License](LICENSE).
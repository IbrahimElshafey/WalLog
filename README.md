# Simple Log 
A single-threaded binary log that supports fixed and variable length log records.

# Settings 
* RecordLength: if -1 then it's variable length log record
* PageSize: page size in bytes

# How to use
You initialize a log `var log = new Log('<folder path>',settings);` 
```C#
await log.Write(<byte array>);
```

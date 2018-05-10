# :large_blue_diamond: UpdateManager for DBeaver Portable
> **When you need something but you can't find it... _JUST CREATE IT_** :shipit:

## :large_orange_diamond: Description  
This is my attemp to implement an auto-update and auto-download-jre system to my portable package of DBeaver. This project has been set on-hold due to some issues: this implementation breaks down the "portable aspect" by requiring .net framework installed.
**The implementation is at 80% of completation. It just need few methods and small refactoring.**

## :large_orange_diamond: Features 
- [x] Check and Download Oracle JRE (Need refactoring: unmerge check and download-extract function)
- [x] Runs the correct version of DBeaver according to your OS:
- 64bit can run both 32bit and 64bit. (Compatible with 32bit, 64bit, both-variant package)
- 32bit can run only 32bit packa. (Compatible with 32bit and both-variant package)
- [x] Check for DBeaver updates
- [ ] Download and extract the updated zips.

**ALL the portable features of the package are untouched. This implementation just updates the binaries.**

## :large_orange_diamond: Legal :warning:  
You can fork and do your modification but at least keep the credits!

# AP_STSExtractor
----------
The STS Extractor is an application designed to translate California Standards-based Test for Spanish (STS) assessment information to the resource item and stimuli formats consumable by Smarter Balanced's Test Delivery System (TDS).

The application is a console application written on top of the .NET framework in C#. It is designed to be run on Windows from the command line.

The application requires several inputs:

 - **-i**  The STS assessments generally come as word documents. The STS extractor requires .htm document input. To provide the documents in the correct format, use the "save as" dialog in word to save the STS document as "web page filtered". This will produce the necessary .htm file and resources directory for this project. The input to this command line parameter is the path to that .htm file. **Be sure to rename the file when you save it to something without any special characters!** (Ex. "C:\Projects\SmarterBalanced\Resources\GRADE3.htm")
 - **-o** The name of the output directory that the STS extractor will write out its results to (Ex. "prod-test")    
 - **-b** The bank key to be applied to each item. Currently, the only valid bank keys are 187 & 200. These two values are located in the app.config contained in a comma delineated list. Currently, those two values are the only two corresponding to valid clients in the TDS system. Using other bank keys will cause errors at runtime. In future (as more clients and associated bank keys are added) the app.config may be modified to include those new bank keys as valid inputs. (Ex. "187")     
 - **-s** Seed item ID. The first item extracted will be assigned this seed ID. All other items will receive the subsequent IDs incrementing from this seed. Ensure that your item seed will not produce items with IDs that match existing items. (Ex. "300")     
 - **-g** This is grade level. All items produced will default to this grade. (Ex. "7")

Attempting to run the application without any command line inputs, or running it with the -h flag will print these instructions to the console.
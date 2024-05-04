Number of rows in your table after running the program: 29889

With larger files like a 10GB CSV, I would consider using a more efficient data loading technique such as, for example, stream processing. 
This approach allows the program to read, process, and write data in chunks, significantly reducing the memory consumption.
I would also take advantage of the async/await features in .NET for non-blocking IO operations, enhancing the application's responsiveness and overall system output. 
Finally, rather than attempting to remove duplicates in memory, I would consider using a more scalable approach such as database-level constraints or bulk operation techniques that can handle duplicate entries more efficiently.

I, of course, consider the current state of the project to be imperfect and to have a bunch of places where it should be improved and optimized.

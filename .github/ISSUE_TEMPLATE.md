<!--
BEFORE YOU SUBMIT AN ISSUE:

DO NOT CREATE AN ISSUE FOR A QUESTION - questions are better served in discussions, and can be later raised as an issue if required.
-  discussions - https://github.com/cake-build/cake/discussions

DELETE EVERYTHING IN THIS COMMENT BLOCK

TEMPLATE FOR BUG REPORTS:
-->

### What You Are Seeing?

### What is Expected?

### What version of Cake are you using?

### Are you running on a 32 or 64 bit system?

### What environment are you running on?  Windows? Linux? Mac?

### Are you running on a CI Server?  If so, which one?

<!--
If possible, provide a link to the failing build.
-->

### How Did You Get This To Happen? (Steps to Reproduce)

<!--

Can you point us to a project where this problem occurs?  i.e. a public GitHub Repo, where we can try to reproduce the problem, and help with debugging?

-->

### Output Log
<!--
When including the log information, please ensure you have run the command with --verbosity=diagnostic. It provides important information for determining an issue.

If running Cake.exe directly, the parameter is passed in as --verbosity=diagnostic
If running Cake via the bootstrapper, the parameter is -Verbosity Diagnostic

- Make sure there is no sensitive data shared.
- We need ALL output, not just what you may believe is relevant.

In order to rule out potential issues with the compilation of your Cake script, it would be very helpful if you could try running the script with some additional parameters.

The first, when compiling on Windows, would be to use the -Mono flag.  This can be passed into both Cake.exe and the Bootstrapper.  This will allow verification that it isn't a problem specific to one compiler or the other.

The second, would be to use the latest version of Roslyn.  This can be done using the -Experimental flag, which again can be passed to both Cake.exe and the bootstrapper.

If you can provide the output from the above when submitting an issue, this would be tremendously useful!
-->

GIST LINK - Please create a gist and link to that gist here

OR

~~~sh
PLACE LOG CONTENT HERE
~~~

<!--
TEMPLATE FOR FEATURE REQUESTS:

It's a blank slate, have fun!
-->

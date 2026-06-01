# Stack up thouse PRs without leaving the GitHub command line

---

## Why interested in this?

- Developer flow
  - no waiting for a review before moving on to next part of task

- Reviewer flow
  - make small logical units that are easy to review

- Build system flow
  - have a clear point where there is a complete unit of change where (some) testing makes sense

---

If I will have to wait, I am tempted to just keep adding more and more changes to the branch

The developer can tell me a story via the commits, but I'd rather have a story through PRs where there is a proper explanation of this part of the story

Our build systems currently leap onto individual commits and do a full build, including building the installers far too often for me.

---

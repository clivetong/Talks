# Stack up those PRs without leaving the GitHub command line

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

Stacked PRs are likely not friendly to our current build system configurations; there could be a lot of rebasing if you have a deep stack, and the systems will enjoy building and testing the installers many times.

installers == any set of tests that hardly ever fails, or part of the product build that I don't need t use to make progress

---

## So what are stacked PRs

Quick diagram from [www.stacking.dev](https://www.stacking.dev/)

![non-stacking](images/non-stacking.png)

![stacking](images/stacked.png)

---

## Concepts

Think of parts of the stack as branches which are based off each other

[Git supports --update-refs](https://andrewlock.net/working-with-stacked-branches-in-git-is-easier-with-update-refs/) but it isn't first class support

---

## Advert from Big Tech

The [Pragmatic Engineer](https://newsletter.pragmaticengineer.com/p/stacked-diffs) says:

*One of my biggest personal “wow” moments during my time at Uber as a developer, was when I began using stacked diffs. Stacking refers to breaking down a pull request (PR) for a feature into several, smaller PRs which all depend on each other – hence the term “stacked”. It might sound counterintuitive, but this workflow is incredibly efficient by making it easier to review and modify PRs – or “diffs” as we called them at Uber.*

---

## GitHub have now added some support

- uses branches and PRs
- maintains metadata to record how these branches form a stack
- adds a command line to allow you to work with the stack not at the branch level 




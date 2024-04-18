# Contribution Guidelines
This project welcomes contributions and suggestions! Before you contribute, please take a moment to read and understand the following contribution guidelines:

## Contributions can be made via a pull request
To maintain transparency, accountability, and ensure proper review of all changes, we require that contributions be made via pull requests. Pull requests provide a structured way to propose and discuss modifications to this repo before they are merged into the main branch.

When submitting a pull request, please provide a clear and concise description of the changes implemented, along with any relevant context or reasoning. This allows reviewers to understand the purpose and impact of the proposed changes fully.

For more information on how to make a pull request, please read [here](./how_to_pull_request.md).

## Assets must be generic
All contributions - solutions, screenshots, videos, etc., must be void of any customer names, logos, or references of any sort. Please use generic, ficticious names in assets you plan to contribute.

## Assets must not contain personal "secrets"
All contributions must not contain any sensitive information - passwords, credentials, Azure connection strings, etc. Please ensure that your assets do not contain any sensitive information before contributing them to this repo.

## No large binary files
We kindly request that you refrain from adding large binary files to the repository. Git, while an exceptional version control system, is not optimized for handling large binary files efficiently. These files can significantly bloat the repository size, making it cumbersome to clone, fetch, and manage for all contributors.

To maintain the integrity and performance of our repository, we encourage you to explore alternative solutions for storing and sharing large binary files, such as Git LFS (Large File Storage) or cloud storage services. For more information, please reach out to the administrators of this repository.

## Contributions must adhere to the outlined structure
To maintain a level of organization and richness in our demo repository, we request that all contributions adhere to the outlined structure described below:

### 1: Folder Structure
Your contribution will be placed in its own unique folder within the `demos/` folder of the repo. That folder must minimally contain a single `README.md` file.
```
demos/
├─ your_cool_new_demo/
│  ├─ README.md
├─ medicaid_chatbot/
│  ├─ README.md
├─ dot_inspections/
│  ├─ README.md
```

### 2: README.md Structure
The `README.md` file within your folder acts as a plain text file (written in [Markdown](https://www.markdownguide.org/getting-started/)) that describes the asset(s) to the community. Your `README.md` file must have the following structure:
- Title and description (what is this?)
- The asset's purpose (where can it be used and by whom? i.e. "This inspection app is intended to alleviate inspection backlogs for Department of Transportations")
- A clear description of every asset being provided (describe what you are providing - i.e. each solution file, code file, sample data, dependencies, etc.)
- Who made this? (credit yourself and anyone you collaborated with)
- Version history (if applicable)

Please see [this GCC & Azure Commercial integration demo](./demos/GCC-to-Commercial/) as an example.

### 3: Add your contribution to all-up demos catalog page
After creating your unique folder and putting your assets there, please add your contribution to the [demos page directory of assets](./demos/). Find the SLG service category that is most appropriate for your contribution (i.e. PS&J, HHS, etc.) and add a row to that table, linking directly to your new contribution.

## Legal
Most contributions require you to agree to a Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
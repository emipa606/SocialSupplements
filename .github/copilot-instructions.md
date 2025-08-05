# GitHub Copilot Instruction File for "Social Supplements (Continued)"

## Mod Overview and Purpose

The "Social Supplements (Continued)" mod is an optional expansion to the "Medical Supplements" mod for RimWorld. It introduces additional social items and drugs that can be used within the game, enhancing the social simulation aspect of the gameplay. This mod requires the "Medical Supplements" mod to function correctly and aims to provide a balanced addition to the base game.

## Key Features and Systems

- **Social Items:** Introduces new drugs that can affect mood, designed to integrate with RimWorld's vanilla theme while allowing for customization and refinement.
- **Research Projects:** Includes new research projects to unlock these social supplements and bulk recipes, allowing a more efficient production of drugs like Pekoe tea, flake, yayo, go-juice, wake-up, and smokeleaf joints.
- **Compatibility:** The mod is save-game compatible, although users need to reassign new drug policies when implemented partway through a game.
- **Bulk Recipes:** Adds bulk recipes for vanilla drugs to streamline production.
- **Optional Mod:** Can work alongside other mods like Apothecary, RimCuisine, Seeds Please, and Smokeleaf Industry, among others.

## Coding Patterns and Conventions

- **C# Classes and Methods:** The mod is written in C# targeting .NET Framework version 4.7.2 and 4.8. Key classes include drug-related components and patching functionality.
    - Example Classes: `CanIngestForJoy_PostPatch`, `CompProperties_SSVapeUses`, `Gizmo_VapeStatus`
    - Use PascalCase for class names and methods.
- **Internal Logic:** Utilize utility classes like `SSVapeUtility` to encapsulate specific logics such as vaping effects.

## XML Integration

- **XML Files:** Integration of XML files is crucial for defining new drugs, recipes, and research projects. Ensure XML definitions are consistent with existing RimWorld mod structures.
- **Naming Conventions:** Use descriptive names for XML def names to maintain readability and compatibility.

## Harmony Patching

- **Harmony Setup:** Utilize the `HarmonySetup` class to apply Harmony patches, enabling modifications to core game behavior without directly altering the game's source code.
- **Postfix Methods:** Use postfix methods to extend or alter existing game functionalities regarding drug ingestion and mood effects.

## Suggestions for Copilot

- **Code Suggestions:** Encourage Copilot to assist with routine coding tasks such as implementing interface methods, class constructors, and repetitive logic structures.
- **XML Code Completion:** Suggest XML snippets for new drug definitions and tagging conventions.
- **Harmony Patching:** Ideal for suggesting patching logic, using `HarmonyPrefix`, `HarmonyPostfix`, or `HarmonyTranspiler`.

## Resources and References

- **Documentation:** Consult the PDF document "Social Supplements Notes" for a detailed understanding of all features and systems offered by the mod.
- **Community Contributions:** Acknowledge contributions such as artwork by Oskar Potocki and translations, utilizing the provided (CC BY-NC-SA 4.0 license).
- **Compatibility Guides:** Follow provided guidelines for ensuring mod compatibility with other popular mods like VGP Vegetable Garden and others.

By following these guidelines and suggestions, contributors and GitHub Copilot can effectively enhance and maintain the Social Supplements mod, ensuring a smooth user experience and consistent functionality.

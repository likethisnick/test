import { createListCollection, Input, Select, SelectContent, SelectItem, SelectLabel, SelectRoot, SelectTrigger, SelectValueText } from "@chakra-ui/react";

export default function Filters({filter, setFilter}) {
    return (
        <div>
            <Input 
                placeholder="Search" 
                onChange={(e) => setFilter({...filter, search: e.target.value })}
            />
            <SelectRoot collection={options} color="black"
            onValueChange={(e) => setFilter({ ...filter, sortOrder: e.value[0] })} >
            <SelectLabel />
            <SelectTrigger>
                <SelectValueText />
            </SelectTrigger>
            <SelectContent backgroundColor={"white"}>
            {options.items.map((option) => (
                <SelectItem item={option} key={option.value}>
                    {option.label}
                </SelectItem>
                ))}
            </SelectContent>
            </SelectRoot>
        </div>
    );
}

const options = createListCollection({
    items: [
      { label: "new", value: "desc" },
      { label: "old", value: "asc" },
    ],
  })
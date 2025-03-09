import { Button, Input, Textarea, useStatStyles } from "@chakra-ui/react";
import { useState } from "react";

export default function CreateUserForm({onCreate}) {
  const[note, setNote] = useState();

const onSubmit = (e) => {
  e.preventDefault();
  setNote(null);
  onCreate(note);
};

  return (
    <form className="w-full flex flex-col gap-3" onSubmit={onSubmit}>
      <h3>Note creation</h3>
      <Input placeholder='Text' color="black"
      value = {note?.title ?? ""}
      onChange={(e) => setNote({...note, title: e.target.value})}></Input>
      <Textarea placeholder="Description" color="black"
      value = {note?.description ?? ""}
      onChange={(e) => setNote({...note, description: e.target.value})}></Textarea>
      <Button type="submit" colorScheme="teal">Create</Button>
    </form>
  )
}
import { Button, Input, Textarea, useStatStyles } from "@chakra-ui/react";
import { useState } from "react";
import SurvefyBigButton from "./SurvefyBigButton";

export default function CreateSurveyForm({onCreate}) {
  const[survey, setSurvey] = useState();

const onSubmit = (e) => {
  e.preventDefault();
  setSurvey(null);
  onCreate(survey);
};



  return (
    <form className="w-full flex flex-col gap-3" onSubmit={onSubmit}>
      <h3>Note creation</h3>
      <Input placeholder='Survey Title' color="black"
      value = {survey?.name ?? ""}
      onChange={(e) => setSurvey({...survey, name: e.target.value})}></Input>
      <Textarea placeholder="Description" color="black"
      value = {survey?.description ?? ""}
      onChange={(e) => setSurvey({...survey, description: e.target.value})}></Textarea>
      <Button type="submit" title="submit">Create</Button>
    </form>
  )
}
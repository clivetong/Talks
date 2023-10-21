import { z } from "zod";

const mySchema = z.object({
  firstname: z.string(),
  surname: z.string()
})

const test = mySchema.parse({
  "moo": 2
});


; /////////////////////////////////////////////////////////////


function isString(val: unknown): asserts val is string {
  if (typeof val != "string") throw `${val} is not a string`;
}


const x = {
  string: () => ({
    parse: (arg: unknown): string => {
      isString(arg);
      return arg;
    }
  })
}

const simpleSchema1 = x.string()

const test1 = simpleSchema1.parse("hello");

; //////////////////////////////////////////////////////////


type Parser<T> = {
  parse: (arg:unknown) => T
}

type Schema = Record<string, Parser<any>>;

const y = {

  ...x,

  object: function<S extends Schema>(schema:S) {

    function validateArgs(arg: unknown): asserts arg is {
      [K in keyof S]: ReturnType<S[K]["parse"]>
    }
    {
      // Add runtime checks here ie use schema
    }

    return {
      parse(arg: unknown) {
        validateArgs(arg)
        return arg;
      }
    }
  }
}

const simpleSchema2 = y.object({
  firstname: y.string(),
  surname: y.string()
})

const test2 = simpleSchema2.parse({
  firstname: "Clive",
  surname: "Tong"
})

test2.firstname;



const simpleSchema3 = y.object({
  firstname: y.string(),
  surname: y.string(),
  address: y.object({
    road: y.string(),
    town: y.string()
  })
})

const test3 = simpleSchema3.parse({
  firstname: "Clive",
  surname: "Tong"
})

test3.address.road;



import Type from "typebox"
import Schema from "typebox/schema"

const UserInfoType = Type.Object({
  sub: Type.Number(),
  email: Type.String(),
  preferred_username: Type.String()
})

export const UserInfoSchema = Schema.Compile(UserInfoType)

export type UserInfo = Type.Static<typeof UserInfoType>
